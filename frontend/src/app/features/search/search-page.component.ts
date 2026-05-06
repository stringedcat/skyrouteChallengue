import { Component, computed, signal, inject, OnInit } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { Router } from '@angular/router';
import { LucideAngularModule, MapPin, Calendar, Users, Briefcase, Search } from 'lucide-angular';
import { AirportService } from '../../core/services/airport.service';
import { FlightService } from '../../core/services/flight.service';
import { SearchStateService } from '../../core/services/search-state.service';
import { Airport } from '../../core/models/airport.model';
import { CabinClass } from '../../core/models/search-flights.model';

@Component({
  selector: 'app-search-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, LucideAngularModule],
  templateUrl: './search-page.component.html'
})
export class SearchPageComponent implements OnInit {
  private fb = inject(FormBuilder);
  private airportService = inject(AirportService);
  private flightService = inject(FlightService);
  private searchState = inject(SearchStateService);
  private router = inject(Router);

  readonly airports = signal<Airport[]>([]);
  readonly isLoading = signal(false);
  readonly error = signal<string | null>(null);

  readonly cabinClasses: CabinClass[] = ['Economy', 'Business', 'FirstClass'];

  readonly mapPinIcon = MapPin;
  readonly calendarIcon = Calendar;
  readonly usersIcon = Users;
  readonly briefcaseIcon = Briefcase;
  readonly searchIcon = Search;

  readonly form = this.fb.nonNullable.group({
    originAirportCode: ['', Validators.required],
    destinationAirportCode: ['', Validators.required],
    departureDate: [this.todayIsoDate(), Validators.required],
    passengers: [1, [Validators.required, Validators.min(1), Validators.max(9)]],
    cabinClass: ['Economy' as CabinClass, Validators.required]
  }, { validators: [this.differentAirportsValidator] });

  private selectedOrigin = toSignal(this.form.controls.originAirportCode.valueChanges, { initialValue: '' });
  private selectedDestination = toSignal(this.form.controls.destinationAirportCode.valueChanges, { initialValue: '' });

  readonly originAirports = computed(() =>
    this.airports().filter(a => a.code !== this.selectedDestination())
  );
  readonly destinationAirports = computed(() =>
    this.airports().filter(a => a.code !== this.selectedOrigin())
  );

  ngOnInit(): void {
    this.airportService.getAll().subscribe({
      next: (airports) => this.airports.set(airports),
      error: () => this.error.set('Failed to load airports. Please refresh the page.')
    });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isLoading.set(true);
    this.error.set(null);

    const request = this.form.getRawValue();

    this.flightService.search(request).subscribe({
      next: (response) => {
        this.searchState.setSearchResults(request, response);
        this.router.navigate(['/results']);
      },
      error: (err) => {
        this.error.set(err.error?.message || 'Search failed. Please try again.');
        this.isLoading.set(false);
      },
      complete: () => this.isLoading.set(false)
    });
  }

  todayIsoDate(): string {
    return new Date().toISOString().split('T')[0];
  }

  private differentAirportsValidator(control: AbstractControl): ValidationErrors | null {
    const origin = control.get('originAirportCode')?.value;
    const destination = control.get('destinationAirportCode')?.value;
    if (origin && destination && origin === destination) {
      return { sameAirport: true };
    }
    return null;
  }
}
