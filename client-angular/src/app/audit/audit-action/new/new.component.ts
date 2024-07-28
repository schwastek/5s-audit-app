import { Component, computed, input, model, viewChild } from '@angular/core';
import { AuditActionFormComponent } from '../form/form.component';
import { AuditActionButtonDeleteComponent } from '../button-delete/button-delete.component';
import { ApiSaveAuditActionRequest } from '../../../api/models';
import { v4 as uuidv4 } from 'uuid';
import { AuditActionSingleComponent } from '../single/single.component';

@Component({
  selector: 'app-audit-action-new',
  standalone: true,
  imports: [
    AuditActionFormComponent,
    AuditActionButtonDeleteComponent,
    AuditActionSingleComponent
  ],
  templateUrl: './new.component.html'
})
export class AuditActionNewComponent {

  // Inputs
  auditId = input.required<string>();
  actions = model<ApiSaveAuditActionRequest[]>([]);

  formComponent = viewChild.required(AuditActionFormComponent);
  form = computed(() => this.formComponent().form);

  // Status
  isSaving = false;

  async onSave(actionDescription: string) {
    if (this.form().invalid) return;

    this.isSaving = true;

    const action = {
      actionId: uuidv4(),
      auditId: this.auditId(),
      description: actionDescription,
      isComplete: false
    };

    this.actions.update((actions) => {
      return [...actions, action];
    });

    this.isSaving = false;

    this.form().reset();
  }

  onDelete(actionId: string) {
    this.actions.update((actions) => {
      return actions.filter((action) => action.actionId !== actionId);
    });
  }
}
