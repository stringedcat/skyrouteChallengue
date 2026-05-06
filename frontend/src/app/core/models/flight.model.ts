export interface Money {
  amount: number;
  currency: string;
}

export interface FlightResult {
  flightId: string;
  provider: string;
  flightNumber: string;
  originCode: string;
  destinationCode: string;
  departureTime: string;
  arrivalTime: string;
  durationMinutes: number;
  cabinClass: string;
  pricePerPassenger: Money;
  totalPrice: Money;
  passengerCount: number;
}
