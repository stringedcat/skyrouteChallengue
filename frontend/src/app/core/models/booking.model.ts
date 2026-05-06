import { CabinClass } from './search-flights.model';
import { FlightResult, Money } from './flight.model';

export interface PassengerInput {
  fullName: string;
  email: string;
  documentNumber: string;
}

export interface CreateBookingRequest {
  flightId: string;
  provider: string;
  departureDate: string;
  originAirportCode: string;
  destinationAirportCode: string;
  cabinClass: CabinClass;
  passengers: PassengerInput[];
}

export interface PassengerSummary {
  fullName: string;
  email: string;
}

export interface PriceBreakdown {
  pricePerPassenger: Money;
  passengerCount: number;
  totalPrice: Money;
}

export interface BookingConfirmation {
  bookingReference: string;
  status: string;
  createdAt: string;
  flight: FlightResult;
  passengers: PassengerSummary[];
  priceBreakdown: PriceBreakdown;
}
