import { Component, output } from '@angular/core';

@Component({
  selector: 'app-audit-action-button-incomplete',
  standalone: true,
  imports: [],
  templateUrl: './button-incomplete.component.html'
})
export class AuditActionButtonIncompleteComponent {

  // Events
  click = output();

  onClick() {
    this.click.emit();
  }
}
