import { Component, output } from '@angular/core';

@Component({
  selector: 'app-audit-action-button-delete',
  standalone: true,
  imports: [],
  templateUrl: './button-delete.component.html'
})
export class AuditActionButtonDeleteComponent {

  // Events
  click = output();

  onClick() {
    this.click.emit();
  }
}
