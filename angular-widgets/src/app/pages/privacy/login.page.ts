import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.page.html',
  styleUrl: './login.page.css'
})
export class LoginPage {
  loginForm: FormGroup;
  isLoading = false;
  serverError = '';

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.loginForm.invalid) return;

    this.isLoading = true;
    this.serverError = '';

    // Переконайся, що в AuthService є метод login()
    this.authService.login(this.loginForm.value).subscribe({
      next: () => {
        // Після успішного входу (коли сервер встановив Cookie), 
        // перенаправляємо на сторінку категорій або завдань
        this.router.navigate(['/categories']); 
      },
      error: (err) => {
        this.isLoading = false;
        // Зазвичай при логіні сервер повертає просту помилку "Невірний логін або пароль"
        this.serverError = err.error?.message || 'Помилка авторизації. Перевірте дані.';
      }
    });
  }
}