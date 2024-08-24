import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AccountService } from '../account.service';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { ValidationMessagesComponent } from '../../shared/components/validation-messages/validation-messages.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    ValidationMessagesComponent
  ],
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit {
  username = new FormControl('', { validators: [Validators.required, Validators.email], nonNullable: true });
  password = new FormControl('', { validators: [Validators.required], nonNullable: true });

  form = new FormGroup({
    username: this.username,
    password: this.password
  });

  returnUrl!: string;
  errorMessage: string | null = null;
  isSubmitting: boolean = false;

  constructor(
    private accountService: AccountService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.returnUrl = this.activatedRoute.snapshot.queryParamMap.get('returnUrl') || '/audits';
  }

  onSubmit() {
    this.isSubmitting = true;
    this.errorMessage = null;

    this.form.disable();

    this.accountService.login(this.username.value, this.password.value).subscribe({
      next: () => {
        this.router.navigateByUrl(this.returnUrl);
      },
      error: (error: HttpErrorResponse) => {
        console.log('error', error);
        this.isSubmitting = false;
        this.errorMessage = error.message;
      }
    });

    this.isSubmitting = false;
    this.form.enable();
  }
}
