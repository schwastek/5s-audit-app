import { Component, OnInit } from '@angular/core';
import { AuditService } from '../audit.service';
import { PaginatedResult } from '../../shared/models/pagination';
import { RouterLink } from '@angular/router';
import { PaginationComponent } from '../../shared/components/pagination/pagination.component';
import { AuditListItemDto } from '../models/audit.models';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-audit-list',
  templateUrl: './audit-list.component.html',
  standalone: true,
  imports: [
    AsyncPipe,
    RouterLink,
    PaginationComponent
  ]
})
export class AuditListComponent implements OnInit {
  public audits = new PaginatedResult<AuditListItemDto>();
  public error: string | null = null;
  public isLoading = false;

  constructor(private auditService: AuditService) {}

  ngOnInit() {
    this.getAudits(1);
  }

  private getAudits(pageNumber: number) {
    this.isLoading = true;

    this.auditService.getAudits(pageNumber).subscribe({
      next: (audits) => {
        this.audits = audits;
      },
      error: (_) => {
        this.error = 'Failed to fetch audits.';
        this.isLoading = false;
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }

  onPageChange(pageNumber: number) {
    // Items are hidden during page changes until loading is complete.
    this.audits.items = [];
    this.getAudits(pageNumber);
  }
}
