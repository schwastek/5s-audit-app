import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AccountService } from '../account.service';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
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

  handleSubmit() {
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

  isControlInvalid(controlName: string): boolean {
    const control = this.form.get(controlName) as FormControl;

    return control.invalid && (control.dirty || control.touched);
  }

  getAriaValidationFeedback(controlName: string) {
    // For invalid fields, ensure that the invalid feedback/error message
    // is associated with the relevant form field using aria-describedby
    // (noting that this attribute allows more than one id to be referenced).
    const isInvalid = this.isControlInvalid(controlName);
    const feedback: string[] = [];

    if (isInvalid) {
      const control = this.form.get(controlName) as AbstractControl;
      const controlErrors = control.errors ?? {};
      Object.keys(controlErrors).forEach((ruleName) => {
        const id = this.getIdForValidationFeedback(controlName, ruleName);
        feedback.push(id);
      });
    }

    const result = feedback.length > 0 ? feedback.join(' ') : null;

    return result;
  }

  getIdForValidationFeedback(controlName: string, ruleName: string) {
    controlName = controlName.toLowerCase();
    ruleName = ruleName.toLowerCase();

    // => 'validation-feedback-for-username-required'
    return `validation-feedback-for-${controlName}-${ruleName}`;
  }
}
