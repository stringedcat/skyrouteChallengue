import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BookingConfirmation, CreateBookingRequest } from '../models/booking.model';
import { API_CONFIG } from '../tokens/api-config.token';

@Injectable({ providedIn: 'root' })
export class BookingService {
  private http = inject(HttpClient);
  private config = inject(API_CONFIG);

  create(request: CreateBookingRequest) {
    return this.http.post<BookingConfirmation>(
      `${this.config.baseUrl}/api/bookings`,
      request
    );
  }

  getByReference(reference: string) {
    return this.http.get<BookingConfirmation>(
      `${this.config.baseUrl}/api/bookings/${reference}`
    );
  }
}
