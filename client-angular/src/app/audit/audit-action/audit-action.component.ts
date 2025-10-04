import { Component, inject, input, model, signal } from '@angular/core';
import { ApiSaveAuditActionRequest, ApiUpdateAuditActionRequest } from '../../api/models';
import { AuditService } from '../audit.service';
import { v4 as uuidv4 } from 'uuid';
import { firstValueFrom, map } from 'rxjs';
import { toSignal } from '@angular/core/rxjs-interop';
import { FormControl, FormGroup, FormGroupDirective, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoadingButtonDirective } from '../../shared/components/loading-button/loading-button.directive';
import { ValidationMessagesComponent } from '../../shared/components/validation-messages/validation-messages.component';

export type SaveOrUpdateAuditAction = ApiSaveAuditActionRequest & ApiUpdateAuditActionRequest;

@Component({
  selector: 'app-audit-action',
  imports: [
    ReactiveFormsModule,
    LoadingButtonDirective,
    ValidationMessagesComponent
  ],
  templateUrl: './audit-action.component.html'
})
export class AuditActionComponent {
  private auditService = inject(AuditService);

  // Inputs
  isEditForm = input.required<boolean>();
  auditId = input.required<string>();
  actions = model<SaveOrUpdateAuditAction[]>([]);

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
  private isUpdatingMap = signal(new Map<string, boolean>());

  async onSave(form: FormGroupDirective) {
    if (form.invalid) return;

    this.isSaving.set(true);

    const action: SaveOrUpdateAuditAction = {
      auditActionId: uuidv4(),
      auditId: this.auditId(),
      description: this.description.value,
      isComplete: false
    };

    if (this.isEditForm()) {
      const response = await firstValueFrom(this.auditService.saveAuditAction(action));
      action.auditActionId = response.auditActionId;
      action.auditId = response.auditId;
      action.description = response.description;
      action.isComplete = response.isComplete;
    }

    this.actions.update((actions) => {
      return [...actions, action];
    });

    this.isSaving.set(false);

    // Use `resetForm()` to reset `submitted` flag and remove the `ng-submitted` class from the form.
    // `FormGroup` has only the `reset()` method. The `resetForm()` method is available only on `FormGroupDirective`.
    form.resetForm();
  }

  async onDelete(auditActionId: string) {
    this.updateStatus(auditActionId, true);

    if (this.isEditForm()) {
      await firstValueFrom(this.auditService.deleteAuditAction(auditActionId));
    }

    this.actions.update((actions) => {
      return actions.filter((action) => action.auditActionId !== auditActionId);
    });

    this.updateStatus(auditActionId, false);
  }

  async onComplete(action: SaveOrUpdateAuditAction) {
    this.updateStatus(action.auditActionId, true);

    const request: ApiUpdateAuditActionRequest = {
      description: action.description,
      isComplete: true
    };

    await firstValueFrom(this.auditService.updateAuditAction(action.auditActionId, request));

    this.actions.update((actions) => {
      const i = actions.findIndex((a) => a.auditActionId === action.auditActionId);
      const updated = { ...actions[i], isComplete: true };

      return [...actions.slice(0, i), updated, ...actions.slice(i + 1)];
    });

    this.updateStatus(action.auditActionId, false);
  }

  async onIncomplete(action: SaveOrUpdateAuditAction) {
    this.updateStatus(action.auditActionId, true);

    const request: ApiUpdateAuditActionRequest = {
      description: action.description,
      isComplete: false
    };

    await firstValueFrom(this.auditService.updateAuditAction(action.auditActionId, request));

    this.actions.update((actions) => {
      const i = actions.findIndex((a) => a.auditActionId === action.auditActionId);
      const updated = { ...actions[i], isComplete: false };

      return [...actions.slice(0, i), updated, ...actions.slice(i + 1)];
    });

    this.updateStatus(action.auditActionId, false);
  }

  isUpdating(auditActionId: string) {
    return this.isUpdatingMap().get(auditActionId) ?? false;
  }

  private updateStatus(actionId: string, status: boolean) {
    this.isUpdatingMap.update((statuses) => {
      statuses.set(actionId, status);
      return new Map(statuses);
    });
  }
}
