import { Component, inject } from '@angular/core'; // Імпортуємо inject
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from './services/auth.service'; 
import { Observable } from 'rxjs/internal/Observable';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent {
  // Використовуємо inject() замість конструктора. 
  // Це чистіший підхід в Angular, який вирішує проблему з чергою ініціалізації.
  public authService = inject(AuthService);

 isLoggedIn$: Observable<boolean> =this.authService.isLoggedIn$; // Ініціалізуємо змінну

  // 2. Присвоюємо значення всередині конструктора, коли authService вже готовий
  constructor() {
   
  }

  onLogout() {
    console.log('Вихід користувача');
    this.authService.logout().subscribe({
      next: () => {console.log('Вихід успішний');window.location.reload();},
      error: (err) => console.error('Помилка при виході', err)
    });
  }
}