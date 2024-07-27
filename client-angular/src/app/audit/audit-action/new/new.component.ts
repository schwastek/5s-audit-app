import { Component, computed, input, signal, viewChild } from '@angular/core';
import { AuditActionFormComponent } from '../form/form.component';
import { AuditActionButtonIncompleteComponent } from '../button-incomplete/button-incomplete.component';
import { AuditActionButtonCompleteComponent } from '../button-complete/button-complete.component';
import { AuditActionButtonDeleteComponent } from '../button-delete/button-delete.component';
import { AuditActionStatusComponent } from '../status/status.component';
import { ApiSaveAuditActionRequest } from '../../../api/models';
import { v4 as uuidv4 } from 'uuid';

@Component({
  selector: 'app-audit-action-new',
  standalone: true,
  imports: [
    // TODO: Add:
    // LoadingButtonDirective,
    AuditActionFormComponent,
    AuditActionButtonIncompleteComponent,
    AuditActionButtonCompleteComponent,
    AuditActionButtonDeleteComponent,
    AuditActionStatusComponent
  ],
  templateUrl: './new.component.html'
})
export class AuditActionNewComponent {

  // Inputs
  auditId = input.required<string>();
  formComponent = viewChild(AuditActionFormComponent);
  form = computed(() => this.formComponent()?.form);
  description = computed(() => this.formComponent()?.description.value);

  // Actions
  actions = signal<ApiSaveAuditActionRequest[]>([]);

  // Status
  isSaving = false;
  isFormOpened = false;

  openForm() {
    this.isFormOpened = true;
  }

  closeForm() {
    this.isFormOpened = false;
  }

  onSave() {

    // The form must exist because it is visible to the user when the save action is available.
    const form = this.form()!;
    const description = this.description()!;

    if (form.invalid) return;

    this.isSaving = true;

    const action = {
      actionId: uuidv4(),
      auditId: this.auditId(),
      description: description,
      isComplete: false
    };

    this.actions.update((actions) => {
      return [...actions, action];
    });

    this.isSaving = false;

    form.reset();

    this.closeForm();
  }

  onDelete(actionId: string) {
    this.actions.update((actions) => {
      return actions.filter((action) => action.actionId !== actionId);
    });
  }
}
