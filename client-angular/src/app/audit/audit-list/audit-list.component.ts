import { Component } from '@angular/core';
import { Audit } from '../../api/models/audit';
import { AuditService } from '../audit.service';
import { PaginatedResult } from '../../api/models/pagination';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-audit-list',
  templateUrl: './audit-list.component.html',
  styleUrls: ['./audit-list.component.scss'],
  standalone: true,
  imports: [CommonModule, RouterLink]
})
export class AuditListComponent {
  audits: PaginatedResult<Audit> = new PaginatedResult<Audit>();

  constructor(private auditService: AuditService) {}

  ngOnInit() {
    this.getAudits();
  }

  getAudits() {
    this.auditService.getAudits()
      .subscribe(audits => this.audits = audits);
  }
}
