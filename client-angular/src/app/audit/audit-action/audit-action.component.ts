import { Component } from '@angular/core';

@Component({
  selector: 'app-audit-action',
  standalone: true,
  imports: [],
  templateUrl: './audit-action.component.html',
  styleUrl: './audit-action.component.scss'
})
export class AuditActionComponent {
  isEditMode: boolean = true;
  descriptionMaxLength: number = 280;

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
