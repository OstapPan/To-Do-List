import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable,tap } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  // Базовий URL до твого AuthController
  private authUrl = '/api/auth';

  private loggedIn = new BehaviorSubject<boolean>(!!localStorage.getItem('isLoggedIn'));
  public isLoggedIn$ = this.loggedIn.asObservable();
  constructor(private http: HttpClient) {}

  /**
   * Реєстрація нового користувача.
   * Бекенд автоматично встановлює Cookie при успіху.
   */
  register(data: any): Observable<any> {
    return this.http.post(`${this.authUrl}/register`, data);
  }

 login(data: any): Observable<any> {
    return this.http.post(`${this.authUrl}/login`, data).pipe(
      tap(() => {
        localStorage.setItem('isLoggedIn', 'true');
        this.loggedIn.next(true);
      })
    );
  }

  logout(): Observable<any> {
    return this.http.post(`${this.authUrl}/logout`, {}).pipe(
      tap(() => {
        localStorage.removeItem('isLoggedIn');
        this.loggedIn.next(false);
      })
    );
  }
}