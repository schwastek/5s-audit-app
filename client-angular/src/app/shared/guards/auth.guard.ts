import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { defaultIfEmpty, map } from 'rxjs';
import { AccountService } from '../../account/account.service';

export const authGuard: CanActivateFn = (_, state) => {
  const accountService: AccountService = inject(AccountService);
  const router: Router = inject(Router);

  return accountService.currentUser$.pipe(
    // Use default value for when Observable is empty (never emitted any value),
    // otherwise `map` operator won't be executed.
    defaultIfEmpty(null),
    map((user) => {
      if (user) {
        return true;
      } else {
        router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
        return false;
      }
    })
  );
};
