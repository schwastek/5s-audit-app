import { Component, input } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { map } from 'rxjs';

@Component({
  selector: 'app-audit-action-form',
  standalone: true,
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './form.component.html'
})
export class AuditActionFormComponent {

  // Inputs
  id = input.required<string>();

  // Form
  description = new FormControl<string>('', { nonNullable: true, validators: [Validators.required] });
  form = new FormGroup({
    description: this.description
  });

  // Character counter
  descriptionLength$ = this.description.valueChanges.pipe((
    map((description) => description?.length ?? 0)
  ));
  descriptionLength = toSignal(this.descriptionLength$, { initialValue: 0 });

  // Configuration
  readonly descriptionMaxLength = 280;
}
