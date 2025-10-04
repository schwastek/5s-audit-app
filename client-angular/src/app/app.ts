import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { AccountService } from './account/account.service';
import { HeaderComponent } from './shared/components/header/header.component';
import { ApiConfiguration } from './api/api-configuration';
import { environment } from '../environments/environment';

@Component({
  selector: 'app-root',
  imports: [CommonModule, RouterOutlet, HeaderComponent],
  templateUrl: './app.html'
})
export class App implements OnInit {
  private accountService = inject(AccountService);
  private apiConfiguration = inject(ApiConfiguration);

  protected readonly title = '5S Audit App';

  ngOnInit(): void {
    this.apiConfiguration.rootUrl = environment.apiUrl;
    this.loadCurrentUser();
  }

  private loadCurrentUser() {
    const token = localStorage.getItem('token');
    this.accountService.loadCurrentUser(token).subscribe();
  }
}
