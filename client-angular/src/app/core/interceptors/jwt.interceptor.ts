import { inject } from '@angular/core';
import {
  HttpRequest,
  HttpEvent,
  HttpHandlerFn
} from '@angular/common/http';
import { Observable, take } from 'rxjs';
import { AccountService } from '../../account/account.service';

export function authInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {
  const accountService = inject(AccountService);
  let token: string | undefined;

  accountService.currentUser$.pipe(take(1)).subscribe({
    next: user => token = user?.token
  });

  // Clone the request to add the authentication header.
  if (token) {
    const authReq = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${token}`)
    });

    return next(authReq);
  }

  return next(req);
}
