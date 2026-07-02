import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavComponent } from './nav.component'; // Імпортуємо навігацію

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavComponent], // Додаємо сюди
  template: `
    <app-nav></app-nav>
    <div class="main-content">
      <router-outlet></router-outlet>
    </div>
  `
})
export class App {
  protected readonly title = signal('To-Do List Application');
}