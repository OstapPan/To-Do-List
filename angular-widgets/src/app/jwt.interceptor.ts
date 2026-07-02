import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    // Отримуємо токен (наприклад, з localStorage)
    // В ідеалі це краще робити через інжект AuthService
    const token = localStorage.getItem('jwt_token');

    if (token) {
      // Клонуємо запит, оскільки об'єкт HttpRequest є незмінним (immutable)
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    // Передаємо запит далі по ланцюжку
    return next.handle(request);
  }
}