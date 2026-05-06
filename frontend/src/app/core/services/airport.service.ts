import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Airport } from '../models/airport.model';
import { API_CONFIG } from '../tokens/api-config.token';

@Injectable({ providedIn: 'root' })
export class AirportService {
  private http = inject(HttpClient);
  private config = inject(API_CONFIG);

  getAll() {
    return this.http.get<Airport[]>(`${this.config.baseUrl}/api/airports`);
  }
}
