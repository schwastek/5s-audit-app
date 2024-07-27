import { Component, output } from '@angular/core';

@Component({
  selector: 'app-audit-action-button-complete',
  standalone: true,
  imports: [],
  templateUrl: './button-complete.component.html'
})
export class AuditActionButtonCompleteComponent {

  // Events
  click = output();

  onClick() {
    this.click.emit();
  }
}
