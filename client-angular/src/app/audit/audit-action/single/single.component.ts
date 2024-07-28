import { Component, input } from '@angular/core';
import { ApiSaveAuditActionRequest } from '../../../api/models';

@Component({
  selector: 'app-audit-action-single',
  standalone: true,
  imports: [],
  templateUrl: './single.component.html'
})
export class AuditActionSingleComponent {
  action = input.required<ApiSaveAuditActionRequest>();
}
