import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AccountService } from '../account.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class LoginComponent {
  loginForm = new FormGroup({
    username: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', Validators.required)
  });
  returnUrl: string;
  isSubmitting: boolean = false;

  constructor(
    private accountService: AccountService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  )
  {
    this.returnUrl = this.activatedRoute.snapshot.queryParamMap.get('returnUrl') || '/audits';
  }

  get username() {
    return this.loginForm.get('username') as FormControl<string | null>;
  }

  get password() {
    return this.loginForm.get('password') as FormControl<string | null>;
  }

  checkIsControlInvalid(controlName: string) {
    const control = this.loginForm.get(controlName) as FormControl;
    return control.invalid && (control.dirty || control.touched);
  }

  getCurrentAriaValidationFeedbackForControl(controlName: string) {
    // For invalid fields, ensure that the invalid feedback/error message
    // is associated with the relevant form field using aria-describedby
    // (noting that this attribute allows more than one id to be referenced).
    const isInvalid = this.checkIsControlInvalid(controlName);
    const feedback: string[] = [];

    if (isInvalid) {
      const control = this.loginForm.get(controlName) as FormControl;
      const controlErrors = control.errors ?? {};
      Object.keys(controlErrors).forEach((ruleName) => {
        const id = this.getIdForValidationFeedbackForRule(controlName, ruleName);
        feedback.push(id);
      });
    }

    const result = feedback.length > 0 ? feedback.join(' ') : null;

    return result;
  }

  getIdForValidationFeedbackForRule(controlName: string, ruleName: string) {
    controlName = controlName.toLowerCase();
    ruleName = ruleName.toLowerCase();

    // => 'validation-feedback-for-username-required'
    return `validation-feedback-for-${controlName}-${ruleName}`;
  }

  async onSubmit() {
    this.isSubmitting = true;

    this.accountService.login(this.username.value, this.password.value).subscribe({
      next: () => this.router.navigateByUrl(this.returnUrl),
      error: error => {
        this.isSubmitting = false;
      }
    });
  }

  delay(ms: number) {
    return new Promise( resolve => setTimeout(resolve, ms));
  }
}
