import { Component, input, model, signal } from '@angular/core';
import { ApiSaveAuditActionRequest, ApiUpdateAuditActionRequest } from '../../api/models';
import { AuditService } from '../audit.service';
import { v4 as uuidv4 } from 'uuid';
import { firstValueFrom, map } from 'rxjs';
import { toSignal } from '@angular/core/rxjs-interop';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoadingButtonDirective } from '../../shared/components/loading-button/loading-button.directive';
import { ValidationMessagesComponent } from '../../shared/components/validation-messages/validation-messages.component';

@Component({
  selector: 'app-audit-action',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    LoadingButtonDirective,
    ValidationMessagesComponent
  ],
  templateUrl: './audit-action.component.html'
})
export class AuditActionComponent {

  // Inputs
  isEditForm = input.required<boolean>();
  auditId = input.required<string>();
  actions = model<ApiSaveAuditActionRequest[]>([]);

  // Form - Configuration
  readonly descriptionMaxLength = 280;

  // Form
  description = new FormControl<string>('', { nonNullable: true, validators: [Validators.required, Validators.maxLength(this.descriptionMaxLength)] });
  form = new FormGroup({
    description: this.description
  });

  // Form - Character counter
  descriptionLength$ = this.description.valueChanges.pipe((
    map((description) => description?.length ?? 0)
  ));
  descriptionLength = toSignal(this.descriptionLength$, { initialValue: 0 });

  // Form - Status
  isSaving = signal(false);

  // Actions - Status (one status per action)
  isUpdating = signal(new Map<string, boolean>());

  constructor(
    private auditService: AuditService
  ) { }

  async onSave() {
    if (this.form.invalid) return;

    this.isSaving.set(true);

    let action: ApiSaveAuditActionRequest = {
      actionId: uuidv4(),
      auditId: this.auditId(),
      description: this.description.value,
      isComplete: false
    };

    if (this.isEditForm()) {
      const response = await firstValueFrom(this.auditService.saveAuditAction(action));
      action = response.auditAction;
    }

    this.actions.update((actions) => {
      return [...actions, action];
    });

    this.isSaving.set(false);

    this.form.reset();
  }

  async onDelete(actionId: string) {
    this.updateStatus(actionId, true);

    if (this.isEditForm()) {
      await firstValueFrom(this.auditService.deleteAuditAction(actionId));
    }

    this.actions.update((actions) => {
      return actions.filter((action) => action.actionId !== actionId);
    });

    this.updateStatus(actionId, false);
  }

  async onComplete(action: ApiSaveAuditActionRequest) {
    this.updateStatus(action.actionId, true);

    const request: ApiUpdateAuditActionRequest = {
      actionId: action.actionId,
      description: action.description,
      isComplete: true
    };

    await firstValueFrom(this.auditService.updateAuditAction(request));

    this.actions.update((actions) => {
      const i = actions.findIndex((a) => a.actionId === action.actionId);
      const updated = { ...actions[i], isComplete: true };

      return [...actions.slice(0, i), updated, ...actions.slice(i + 1)];
    });

    this.updateStatus(action.actionId, false);
  }

  async onIncomplete(action: ApiSaveAuditActionRequest) {
    this.updateStatus(action.actionId, true);

    const request: ApiUpdateAuditActionRequest = {
      actionId: action.actionId,
      description: action.description,
      isComplete: false
    };

    await firstValueFrom(this.auditService.updateAuditAction(request));

    this.actions.update((actions) => {
      const i = actions.findIndex((a) => a.actionId === action.actionId);
      const updated = { ...actions[i], isComplete: false };

      return [...actions.slice(0, i), updated, ...actions.slice(i + 1)];
    });

    this.updateStatus(action.actionId, false);
  }

  private updateStatus(actionId: string, status: boolean) {
    this.isUpdating.update((statuses) => {
      statuses.set(actionId, status);
      return new Map(statuses);
    });
  }
}
