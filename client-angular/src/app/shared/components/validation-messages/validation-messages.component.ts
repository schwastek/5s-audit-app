import { Component, input } from '@angular/core';
import { ValidationErrors } from '@angular/forms';

@Component({
  selector: 'app-validation-messages',
  standalone: true,
  imports: [],
  templateUrl: './validation-messages.component.html',
  host: {
    class: 'invalid-feedback'
  }
})
export class ValidationMessagesComponent {
  errors = input<ValidationErrors | null | undefined>();
}
