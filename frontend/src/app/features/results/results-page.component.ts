import { Component, signal, inject, computed, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { LucideAngularModule, Plane, Briefcase, Search } from 'lucide-angular';
import { SearchStateService } from '../../core/services/search-state.service';
import { FlightResult } from '../../core/models/flight.model';

type SortOption = 'price-asc' | 'price-desc' | 'duration-asc' | 'departure-asc';

@Component({
  selector: 'app-results-page',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, LucideAngularModule],
  template: `
    <div class="mx-auto max-w-5xl px-4 py-8">
      @if (request(); as req) {
        <div class="mb-6 flex items-center justify-between">
          <div>
            <h2 class="text-2xl font-bold text-slate-900">Flight results</h2>
            <p class="mt-1 text-sm text-slate-600">
              {{ req.originAirportCode }} → {{ req.destinationAirportCode }} ·
              {{ req.departureDate }} ·
              {{ req.passengers }} {{ req.passengers === 1 ? 'passenger' : 'passengers' }} ·
              {{ req.cabinClass }}
            </p>
          </div>
          <a routerLink="/search" class="text-sm font-medium text-blue-600 hover:text-blue-700">
            ← Modify search
          </a>
        </div>
      }

      @if (sortedResults().length > 0) {
        <div class="mb-4 flex items-center justify-between">
          <p class="text-sm text-slate-600">{{ sortedResults().length }} flights found</p>
          <div class="flex items-center gap-2">
            <label for="sort" class="text-sm font-medium text-slate-700">Sort by:</label>
            <select
              id="sort"
              [ngModel]="sortOption()"
              (ngModelChange)="sortOption.set($event)"
              class="rounded-md border border-slate-300 px-3 py-1.5 text-sm focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500">
              <option value="price-asc">Price (low to high)</option>
              <option value="price-desc">Price (high to low)</option>
              <option value="duration-asc">Duration (shortest)</option>
              <option value="departure-asc">Departure (earliest)</option>
            </select>
          </div>
        </div>
      }

      @if (sortedResults().length > 0) {
        <div class="space-y-4">
          @for (flight of sortedResults(); track flight.flightId) {
            <div class="rounded-lg border border-slate-200 bg-white p-6 shadow-sm transition-all hover:shadow-md hover:border-blue-200">
              <div class="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
                <div class="flex-1">
                  <div class="flex items-center gap-3">
                    @if (flight.provider === 'GlobalAir') {
                      <span class="rounded-full bg-indigo-100 px-2.5 py-0.5 text-xs font-semibold text-indigo-700">
                        GlobalAir
                      </span>
                    } @else if (flight.provider === 'BudgetWings') {
                      <span class="rounded-full bg-emerald-100 px-2.5 py-0.5 text-xs font-semibold text-emerald-700">
                        BudgetWings
                      </span>
                    } @else {
                      <span class="rounded-full bg-slate-100 px-2.5 py-0.5 text-xs font-semibold text-slate-700">
                        {{ flight.provider }}
                      </span>
                    }
                    <span class="text-sm font-medium text-slate-900">{{ flight.flightNumber }}</span>
                    <span class="inline-flex items-center gap-1 text-xs text-slate-500">
                      <lucide-icon [img]="briefcaseIcon" class="h-3 w-3"></lucide-icon>
                      {{ flight.cabinClass }}
                    </span>
                  </div>

                  <div class="mt-3 flex items-center gap-4">
                    <div>
                      <p class="text-lg font-semibold text-slate-900">{{ formatTime(flight.departureTime) }}</p>
                      <p class="text-sm text-slate-600">{{ flight.originCode }}</p>
                    </div>
                    <div class="flex flex-col items-center text-slate-400 px-2">
                      <span class="text-xs font-medium">{{ formatDuration(flight.durationMinutes) }}</span>
                      <div class="relative w-24 my-1">
                        <div class="absolute inset-0 flex items-center"><div class="w-full border-t border-dashed border-slate-300"></div></div>
                        <div class="relative flex justify-center">
                          <lucide-icon [img]="planeIcon" class="h-4 w-4 text-blue-500 -rotate-45 bg-white px-0.5"></lucide-icon>
                        </div>
                      </div>
                      <span class="text-xs">Direct</span>
                    </div>
                    <div>
                      <p class="text-lg font-semibold text-slate-900">{{ formatTime(flight.arrivalTime) }}</p>
                      <p class="text-sm text-slate-600">{{ flight.destinationCode }}</p>
                    </div>
                  </div>
                </div>

                <div class="flex flex-col items-end gap-2">
                  <div class="text-right">
                    <p class="text-2xl font-bold text-slate-900">
                      {{ flight.totalPrice.currency }} {{ flight.totalPrice.amount.toFixed(2) }}
                      <span class="text-xs font-normal text-slate-500">total</span>
                    </p>
                    <p class="text-xs text-slate-500">
                      {{ flight.pricePerPassenger.currency }} {{ flight.pricePerPassenger.amount.toFixed(2) }} per person
                    </p>
                  </div>
                  <button
                    type="button"
                    (click)="selectFlight(flight)"
                    class="rounded-md bg-blue-600 px-4 py-2 text-sm font-medium text-white hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2">
                    Select
                  </button>
                </div>
              </div>
            </div>
          }
        </div>
      } @else {
        <div class="rounded-lg border border-slate-200 bg-white p-12 text-center">
          <div class="mx-auto mb-4 flex h-16 w-16 items-center justify-center rounded-full bg-slate-100">
            <lucide-icon [img]="searchIcon" class="h-8 w-8 text-slate-400"></lucide-icon>
          </div>
          <h3 class="text-lg font-semibold text-slate-900">No flights found</h3>
          <p class="mt-1 text-sm text-slate-600">Try adjusting your search criteria.</p>
          <a routerLink="/search" class="mt-4 inline-block rounded-md bg-blue-600 px-4 py-2 text-sm font-medium text-white hover:bg-blue-700">
            Modify search
          </a>
        </div>
      }
    </div>
  `
})
export class ResultsPageComponent implements OnInit {
  private searchState = inject(SearchStateService);
  private router = inject(Router);

  readonly planeIcon = Plane;
  readonly briefcaseIcon = Briefcase;
  readonly searchIcon = Search;

  readonly sortOption = signal<SortOption>('price-asc');

  readonly request = this.searchState.lastRequest;
  readonly response = this.searchState.lastResponse;

  readonly sortedResults = computed(() => {
    const results = this.response()?.results ?? [];
    const option = this.sortOption();

    const sorted = [...results];

    switch (option) {
      case 'price-asc':
        return sorted.sort((a, b) => a.totalPrice.amount - b.totalPrice.amount);
      case 'price-desc':
        return sorted.sort((a, b) => b.totalPrice.amount - a.totalPrice.amount);
      case 'duration-asc':
        return sorted.sort((a, b) => a.durationMinutes - b.durationMinutes);
      case 'departure-asc':
        return sorted.sort((a, b) =>
          new Date(a.departureTime).getTime() - new Date(b.departureTime).getTime());
    }
  });

  ngOnInit(): void {
    if (!this.searchState.hasResults()) {
      this.router.navigate(['/search']);
    }
  }

  selectFlight(flight: FlightResult): void {
    this.searchState.selectFlight(flight);
    this.router.navigate(['/booking', flight.flightId]);
  }

  formatDuration(minutes: number): string {
    const hours = Math.floor(minutes / 60);
    const mins = minutes % 60;
    return `${hours}h ${mins}m`;
  }

  formatTime(isoString: string): string {
    return new Date(isoString).toLocaleString('en-US', {
      weekday: 'short',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
      hour12: false
    });
  }
}
