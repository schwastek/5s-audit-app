import { Component, input, output } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { map } from 'rxjs';
import { LoadingButtonDirective } from '../../../shared/components/loading-button/loading-button.directive';

@Component({
  selector: 'app-audit-action-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    LoadingButtonDirective
  ],
  templateUrl: './form.component.html'
})
export class AuditActionFormComponent {

  // Inputs
  isSaving = input<boolean>(false);

  // Events
  save = output<string>();

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

  get isDisabledSave() {
    return this.form.invalid || this.isSaving();
  }

  onSave() {
    this.save.emit(this.description.value);
  }
}
