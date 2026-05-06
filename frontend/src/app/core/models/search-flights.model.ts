import { FlightResult } from './flight.model';

export type CabinClass = 'Economy' | 'Business' | 'FirstClass';

export interface SearchFlightsRequest {
  originAirportCode: string;
  destinationAirportCode: string;
  departureDate: string;
  passengers: number;
  cabinClass: CabinClass;
}

export interface SearchFlightsResponse {
  isInternational: boolean;
  requiredDocument: 'Passport' | 'NationalId';
  results: FlightResult[];
}
