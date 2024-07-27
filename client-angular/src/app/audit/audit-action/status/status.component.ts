import { Component, input } from '@angular/core';

@Component({
  selector: 'app-audit-action-status',
  standalone: true,
  imports: [],
  templateUrl: './status.component.html'
})
export class AuditActionStatusComponent {

  // Inputs
  isComplete = input.required<boolean>();
}
