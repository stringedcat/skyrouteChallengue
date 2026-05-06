import { Component } from '@angular/core';
import { RouterOutlet, RouterLink } from '@angular/router';
import { LucideAngularModule, Plane } from 'lucide-angular';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, LucideAngularModule],
  template: `
    <div class="min-h-screen bg-slate-50 flex flex-col">
      <header class="bg-white border-b border-slate-200">
        <div class="mx-auto max-w-6xl px-4 py-4 flex items-center justify-between">
          <a routerLink="/search" class="flex items-center gap-2 group">
            <div class="flex h-9 w-9 items-center justify-center rounded-lg bg-gradient-to-br from-blue-600 to-blue-500 text-white">
              <lucide-icon [img]="planeIcon" class="h-5 w-5 -rotate-45 transition-transform group-hover:rotate-0"></lucide-icon>
            </div>
            <span class="text-lg font-bold text-slate-900">SkyRoute</span>
          </a>
          <nav class="text-sm text-slate-600">Find your flight</nav>
        </div>
      </header>

      <main class="flex-1">
        <router-outlet />
      </main>

      <footer class="border-t border-slate-200 bg-white py-6">
        <div class="mx-auto max-w-6xl px-4 text-center text-xs text-slate-500">
          SkyRoute · Senior Full-Stack Challenge
        </div>
      </footer>
    </div>
  `
})
export class AppComponent {
  readonly planeIcon = Plane;
}
