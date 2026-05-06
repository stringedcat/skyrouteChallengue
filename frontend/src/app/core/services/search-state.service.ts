import { Injectable, signal, computed } from '@angular/core';
import { SearchFlightsRequest, SearchFlightsResponse } from '../models/search-flights.model';
import { FlightResult } from '../models/flight.model';

@Injectable({ providedIn: 'root' })
export class SearchStateService {
  private _lastRequest = signal<SearchFlightsRequest | null>(null);
  private _lastResponse = signal<SearchFlightsResponse | null>(null);
  private _selectedFlight = signal<FlightResult | null>(null);

  readonly lastRequest = this._lastRequest.asReadonly();
  readonly lastResponse = this._lastResponse.asReadonly();
  readonly hasResults = computed(() => this._lastResponse() !== null);
  readonly selectedFlight = this._selectedFlight.asReadonly();

  setSearchResults(request: SearchFlightsRequest, response: SearchFlightsResponse): void {
    this._lastRequest.set(request);
    this._lastResponse.set(response);
  }

  selectFlight(flight: FlightResult): void {
    this._selectedFlight.set(flight);
  }

  clear(): void {
    this._lastRequest.set(null);
    this._lastResponse.set(null);
    this._selectedFlight.set(null);
  }
}
