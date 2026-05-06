import { Component, signal, inject, computed, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormArray, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { LucideAngularModule, CheckCircle } from 'lucide-angular';
import { SearchStateService } from '../../core/services/search-state.service';
import { BookingService } from '../../core/services/booking.service';
import { CreateBookingRequest, BookingConfirmation } from '../../core/models/booking.model';

@Component({
  selector: 'app-booking-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, LucideAngularModule],
  template: `
    @if (bookingResult() === null) {
      <!-- Step indicator -->
      <div class="mx-auto max-w-3xl px-4 pt-6">
        <ol class="flex items-center justify-center gap-2 text-xs text-slate-500">
          <li class="flex items-center gap-1.5">
            <span class="flex h-6 w-6 items-center justify-center rounded-full bg-slate-200 text-slate-600">1</span>
            <span>Search</span>
          </li>
          <span class="text-slate-300">—</span>
          <li class="flex items-center gap-1.5">
            <span class="flex h-6 w-6 items-center justify-center rounded-full bg-slate-200 text-slate-600">2</span>
            <span>Select</span>
          </li>
          <span class="text-slate-300">—</span>
          <li class="flex items-center gap-1.5 font-semibold text-blue-600">
            <span class="flex h-6 w-6 items-center justify-center rounded-full bg-blue-600 text-white text-xs">3</span>
            <span>Passengers</span>
          </li>
          <span class="text-slate-300">—</span>
          <li class="flex items-center gap-1.5">
            <span class="flex h-6 w-6 items-center justify-center rounded-full bg-slate-200 text-slate-600">4</span>
            <span>Confirm</span>
          </li>
        </ol>
      </div>

      <div class="mx-auto max-w-3xl px-4 py-8">
        <a routerLink="/results" class="mb-4 inline-flex items-center text-sm text-blue-600 hover:text-blue-700">
          ← Back to results
        </a>

        <h2 class="mb-6 text-2xl font-bold text-slate-900">Complete your booking</h2>

        @if (error()) {
          <div class="mb-6 rounded-lg border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
            {{ error() }}
          </div>
        }

        <div class="grid grid-cols-1 gap-6 lg:grid-cols-3">
          <div class="lg:col-span-2">
            @if (flight(); as flightInfo) {
              <form [formGroup]="form" (ngSubmit)="onSubmit()" class="space-y-6">
                <div formArrayName="passengers" class="space-y-4">
                  @for (passengerForm of passengersArray.controls; track $index; let i = $index) {
                    <div class="rounded-lg border border-slate-200 bg-white p-6 shadow-sm" [formGroupName]="i">
                      <h3 class="mb-4 text-lg font-semibold text-slate-900">Passenger {{ i + 1 }}</h3>

                      <div class="space-y-4">
                        <div>
                          <label class="mb-1 block text-sm font-medium text-slate-700">Full name</label>
                          <input
                            type="text"
                            formControlName="fullName"
                            class="w-full rounded-md border border-slate-300 px-3 py-2 text-sm focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-500" />
                          @if (passengerForm.get('fullName')?.touched && passengerForm.get('fullName')?.errors) {
                            <p class="mt-1 text-xs text-red-600">
                              @if (passengerForm.get('fullName')?.errors?.['required']) {
                                Full name is required
                              } @else if (passengerForm.get('fullName')?.errors?.['minlength']) {
                                Minimum 3 characters
                              } @else if (passengerForm.get('fullName')?.errors?.['maxlength']) {
                                Maximum 100 characters
                              }
                            </p>
                          }
                        </div>

                        <div>
                          <label class="mb-1 block text-sm font-medium text-slate-700">Email</label>
                          <input
                            type="email"
                            formControlName="email"
                            class="w-full rounded-md border border-slate-300 px-3 py-2 text-sm focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-500" />
                          @if (passengerForm.get('email')?.touched && passengerForm.get('email')?.errors) {
                            <p class="mt-1 text-xs text-red-600">
                              @if (passengerForm.get('email')?.errors?.['required']) {
                                Email is required
                              } @else if (passengerForm.get('email')?.errors?.['email']) {
                                Invalid email format
                              }
                            </p>
                          }
                        </div>

                        <div>
                          <label class="mb-1 block text-sm font-medium text-slate-700">{{ documentLabel() }}</label>
                          <input
                            type="text"
                            formControlName="documentNumber"
                            [placeholder]="documentPlaceholder()"
                            class="w-full rounded-md border border-slate-300 px-3 py-2 text-sm focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-500" />
                          @if (passengerForm.get('documentNumber')?.touched && passengerForm.get('documentNumber')?.errors) {
                            <p class="mt-1 text-xs text-red-600">
                              @if (passengerForm.get('documentNumber')?.errors?.['required']) {
                                {{ documentLabel() }} is required
                              } @else if (passengerForm.get('documentNumber')?.errors?.['pattern']) {
                                @if (requiredDocument() === 'Passport') {
                                  Passport must be 6-12 alphanumeric characters
                                } @else {
                                  National ID must be 7-9 digits
                                }
                              }
                            </p>
                          }
                        </div>
                      </div>
                    </div>
                  }
                </div>

                <button
                  type="submit"
                  [disabled]="isLoading()"
                  class="w-full rounded-md bg-gradient-to-r from-blue-600 to-blue-500 px-6 py-3 text-sm font-medium text-white hover:from-blue-700 hover:to-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 disabled:opacity-50">
                  @if (isLoading()) {
                    <span class="inline-flex items-center justify-center gap-2">
                      <svg class="h-5 w-5 animate-spin" viewBox="0 0 24 24" fill="none">
                        <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="3"></circle>
                        <path class="opacity-90" fill="currentColor" d="M4 12a8 8 0 0 1 8-8v4a4 4 0 0 0-4 4H4z"></path>
                      </svg>
                      Confirming...
                    </span>
                  } @else {
                    Confirm Booking
                  }
                </button>
              </form>
            }
          </div>

          <div class="lg:col-span-1">
            @if (flight(); as flightInfo) {
              <div class="sticky top-4 rounded-lg border border-slate-200 bg-white p-6 shadow-sm">
                <h3 class="mb-4 text-lg font-semibold text-slate-900">Trip summary</h3>

                <div class="mb-4 space-y-2 border-b border-slate-200 pb-4">
                  <p class="text-sm text-slate-600">{{ flightInfo.provider }} · {{ flightInfo.flightNumber }}</p>
                  <p class="text-sm font-medium text-slate-900">
                    {{ flightInfo.originCode }} → {{ flightInfo.destinationCode }}
                  </p>
                  <p class="text-xs text-slate-600">{{ formatTime(flightInfo.departureTime) }}</p>
                  <p class="text-xs text-slate-600">{{ formatDuration(flightInfo.durationMinutes) }} · {{ flightInfo.cabinClass }}</p>
                </div>

                <div class="space-y-2">
                  <div class="flex justify-between text-sm text-slate-600">
                    <span>{{ flightInfo.pricePerPassenger.currency }} {{ flightInfo.pricePerPassenger.amount.toFixed(2) }} × {{ flightInfo.passengerCount }}</span>
                  </div>
                  <div class="flex justify-between border-t border-slate-200 pt-2 text-base font-bold text-slate-900">
                    <span>Total</span>
                    <span>{{ flightInfo.totalPrice.currency }} {{ flightInfo.totalPrice.amount.toFixed(2) }}</span>
                  </div>
                </div>
              </div>
            }
          </div>
        </div>
      </div>
    } @else {
      <!-- Confirmation state -->
      <!-- Step indicator - all complete -->
      <div class="mx-auto max-w-3xl px-4 pt-6">
        <ol class="flex items-center justify-center gap-2 text-xs">
          <li class="flex items-center gap-1.5 text-blue-600">
            <span class="flex h-6 w-6 items-center justify-center rounded-full bg-blue-600 text-white text-xs">✓</span>
            <span>Search</span>
          </li>
          <span class="text-blue-300">—</span>
          <li class="flex items-center gap-1.5 text-blue-600">
            <span class="flex h-6 w-6 items-center justify-center rounded-full bg-blue-600 text-white text-xs">✓</span>
            <span>Select</span>
          </li>
          <span class="text-blue-300">—</span>
          <li class="flex items-center gap-1.5 text-blue-600">
            <span class="flex h-6 w-6 items-center justify-center rounded-full bg-blue-600 text-white text-xs">✓</span>
            <span>Passengers</span>
          </li>
          <span class="text-blue-300">—</span>
          <li class="flex items-center gap-1.5 font-semibold text-blue-600">
            <span class="flex h-6 w-6 items-center justify-center rounded-full bg-blue-600 text-white text-xs">✓</span>
            <span>Confirmed</span>
          </li>
        </ol>
      </div>

      <div class="mx-auto max-w-2xl px-4 py-12">
        @if (bookingResult(); as confirmation) {
          <div class="rounded-lg border border-green-200 bg-white p-8 text-center shadow-sm">
            <div class="mx-auto mb-4 flex h-16 w-16 items-center justify-center rounded-full bg-green-100">
              <lucide-icon [img]="checkCircleIcon" class="h-10 w-10 text-green-600"></lucide-icon>
            </div>

            <h2 class="text-2xl font-bold text-slate-900">Booking confirmed!</h2>

            <div class="mt-6 rounded-lg border-2 border-dashed border-slate-200 bg-slate-50 p-4">
              <p class="text-xs uppercase text-slate-500">Booking Reference</p>
              <p class="mt-1 text-2xl font-bold tracking-wider text-slate-900 font-mono">{{ confirmation.bookingReference }}</p>
            </div>

            <div class="mt-8 border-t border-slate-200 pt-6 text-left">
              <h3 class="mb-3 text-sm font-semibold uppercase text-slate-500">Flight details</h3>
              <p class="text-sm text-slate-900">
                {{ confirmation.flight.provider }} · {{ confirmation.flight.flightNumber }}
              </p>
              <p class="text-sm text-slate-700">
                {{ confirmation.flight.originCode }} → {{ confirmation.flight.destinationCode }}
              </p>
              <p class="text-xs text-slate-600">
                {{ formatTime(confirmation.flight.departureTime) }}
              </p>
            </div>

            <div class="mt-6 border-t border-slate-200 pt-6 text-left">
              <h3 class="mb-3 text-sm font-semibold uppercase text-slate-500">Passengers</h3>
              <ul class="space-y-1">
                @for (passenger of confirmation.passengers; track passenger.email) {
                  <li class="text-sm text-slate-900">{{ passenger.fullName }} <span class="text-xs text-slate-500">{{ passenger.email }}</span></li>
                }
              </ul>
            </div>

            <div class="mt-6 border-t border-slate-200 pt-6 text-left">
              <h3 class="mb-3 text-sm font-semibold uppercase text-slate-500">Total paid</h3>
              <p class="text-xl font-bold text-slate-900">
                {{ confirmation.priceBreakdown.totalPrice.currency }} {{ confirmation.priceBreakdown.totalPrice.amount.toFixed(2) }}
              </p>
            </div>

            <a routerLink="/search" class="mt-8 inline-block rounded-md bg-blue-600 px-6 py-2.5 text-sm font-medium text-white hover:bg-blue-700">
              Book another flight
            </a>
          </div>
        }
      </div>
    }
  `
})
export class BookingPageComponent implements OnInit {
  private fb = inject(FormBuilder);
  private searchState = inject(SearchStateService);
  private bookingService = inject(BookingService);
  private router = inject(Router);

  readonly checkCircleIcon = CheckCircle;

  readonly flight = this.searchState.selectedFlight;
  readonly request = this.searchState.lastRequest;
  readonly response = this.searchState.lastResponse;

  readonly isLoading = signal(false);
  readonly error = signal<string | null>(null);
  readonly bookingResult = signal<BookingConfirmation | null>(null);

  readonly requiredDocument = computed(() =>
    this.response()?.requiredDocument ?? 'NationalId'
  );

  readonly documentLabel = computed(() =>
    this.requiredDocument() === 'Passport' ? 'Passport Number' : 'National ID'
  );

  readonly documentPlaceholder = computed(() =>
    this.requiredDocument() === 'Passport' ? 'AB123456' : '12345678'
  );

  readonly form = this.fb.group({
    passengers: this.fb.array<FormGroup>([])
  });

  get passengersArray(): FormArray {
    return this.form.get('passengers') as FormArray;
  }

  ngOnInit(): void {
    const flight = this.flight();
    const request = this.request();

    if (!flight || !request) {
      this.router.navigate(['/search']);
      return;
    }

    for (let i = 0; i < request.passengers; i++) {
      this.passengersArray.push(this.createPassengerForm());
    }
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const flight = this.flight();
    const request = this.request();
    if (!flight || !request) return;

    this.isLoading.set(true);
    this.error.set(null);

    const bookingRequest: CreateBookingRequest = {
      flightId: flight.flightId,
      provider: flight.provider,
      departureDate: request.departureDate,
      originAirportCode: request.originAirportCode,
      destinationAirportCode: request.destinationAirportCode,
      cabinClass: request.cabinClass,
      passengers: this.passengersArray.value
    };

    this.bookingService.create(bookingRequest).subscribe({
      next: (result) => {
        this.bookingResult.set(result);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.error.set(err.error?.message || 'Booking failed. Please try again.');
        this.isLoading.set(false);
      }
    });
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

  formatDuration(minutes: number): string {
    const hours = Math.floor(minutes / 60);
    const mins = minutes % 60;
    return `${hours}h ${mins}m`;
  }

  private createPassengerForm(): FormGroup {
    const docPattern = this.requiredDocument() === 'Passport'
      ? /^[a-zA-Z0-9]{6,12}$/
      : /^[0-9]{7,9}$/;

    return this.fb.nonNullable.group({
      fullName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      email: ['', [Validators.required, Validators.email]],
      documentNumber: ['', [Validators.required, Validators.pattern(docPattern)]]
    });
  }
}
