import { Component, inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AccountService } from '../../../account/account.service';
import { User } from '../../../account/models/user';
import { Observable, of } from 'rxjs';
import { AsyncPipe, NgClass } from '@angular/common';

@Component({
  selector: 'app-header',
  imports: [RouterLink, AsyncPipe, NgClass],
  templateUrl: './header.component.html'
})
export class HeaderComponent implements OnInit {
  private accountService = inject(AccountService);

  user$: Observable<User | null> = of(null);

  ngOnInit(): void {
    this.user$ = this.accountService.currentUser$;
  }

  logout() {
    this.accountService.logout();
  }
}
