import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'search', pathMatch: 'full' },
  {
    path: 'search',
    loadComponent: () => import('./features/search/search-page.component').then(m => m.SearchPageComponent)
  },
  {
    path: 'results',
    loadComponent: () => import('./features/results/results-page.component').then(m => m.ResultsPageComponent)
  },
  {
    path: 'booking/:flightId',
    loadComponent: () => import('./features/booking/booking-page.component').then(m => m.BookingPageComponent)
  },
  { path: '**', redirectTo: 'search' }
];
