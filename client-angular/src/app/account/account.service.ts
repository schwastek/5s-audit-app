import { inject, Injectable } from '@angular/core';
import { ReplaySubject, catchError, map, of, throwError } from 'rxjs';
import { environment } from '../../environments/environment';
import { User } from './models/user';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private http = inject(HttpClient);
  private router = inject(Router);

  baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<User | null>(1);
  currentUser$ = this.currentUserSource.asObservable();

  login(email: string | null, password: string | null) {
    return this.http.post<User>(this.baseUrl + '/api/account/login', { email, password })
      .pipe(
        map((user) => {
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
        })
      );
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    window.location.href = '/';
  }

  loadCurrentUser(token: string | null) {
    if (token == null) {
      this.currentUserSource.next(null);
      return of(null);
    }

    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);

    return this.http.get<User>(this.baseUrl + '/api/account', { headers })
      .pipe(
        map((user) => {
          if (user) {
            localStorage.setItem('token', user.token);
            this.currentUserSource.next(user);
            return user;
          } else {
            return null;
          }
        }),
        catchError((error: HttpErrorResponse) => {
          if (error.status === 401) {
            this.router.navigateByUrl('/login');
          }

          return throwError(() => new Error(error.message));
        })
      );
  }
}
