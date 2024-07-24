import { Component } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { map } from 'rxjs';

@Component({
  selector: 'app-audit-action',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './audit-action.component.html',
  styleUrl: './audit-action.component.scss'
})
export class AuditActionComponent {
  isEditMode = false;
  descriptionMaxLength = 280;

  // Form
  description = new FormControl<string | null>(null, { validators: [Validators.required] });
  form = new FormGroup({
    description: this.description
  });

  descriptionLength$ = this.description.valueChanges.pipe((
    map((description) => description?.length ?? 0)
  ));
  descriptionLength = toSignal(this.descriptionLength$, { initialValue: 0 });

  handleDeleteAction(actionId: string, event: Event) {
  }

  handleChange(event: Event) {
    const value = '';
    const description = value.slice(0, this.descriptionMaxLength);
  }

  handleSubmit() {
    this.handleCloseForm();
  }

  handleOpenForm() {
    this.isEditMode = true;
  }

  handleCloseForm() {
    this.isEditMode = false;
  }

  handleSubmitAction(description: string) {
  }
}
