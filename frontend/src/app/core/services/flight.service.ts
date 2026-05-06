import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SearchFlightsRequest, SearchFlightsResponse } from '../models/search-flights.model';
import { API_CONFIG } from '../tokens/api-config.token';

@Injectable({ providedIn: 'root' })
export class FlightService {
  private http = inject(HttpClient);
  private config = inject(API_CONFIG);

  search(request: SearchFlightsRequest) {
    return this.http.post<SearchFlightsResponse>(
      `${this.config.baseUrl}/api/flights/search`,
      request
    );
  }
}
