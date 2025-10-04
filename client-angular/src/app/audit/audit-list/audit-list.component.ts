import { Component, inject, OnInit } from '@angular/core';
import { AuditService } from '../audit.service';
import { PaginatedResult } from '../../shared/models/pagination';
import { RouterLink } from '@angular/router';
import { PaginationComponent } from '../../shared/components/pagination/pagination.component';
import { AuditListItemDto } from '../models/audit.models';
import { LoadingService } from '../../shared/loading/loading.service';
import { LoadingComponent } from '../../shared/loading/loading.component';

@Component({
  selector: 'app-audit-list',
  templateUrl: './audit-list.component.html',
  imports: [
    RouterLink,
    PaginationComponent,
    LoadingComponent
  ],
  providers: [LoadingService]
})
export class AuditListComponent implements OnInit {
  private auditService = inject(AuditService);
  private loadingService = inject(LoadingService);

  audits = new PaginatedResult<AuditListItemDto>();
  error: string | null = null;
  isLoading = this.loadingService.isLoading;

  ngOnInit() {
    this.getAudits(1);
  }

  private getAudits(pageNumber: number) {
    this.loadingService.start();
    this.error = null;

    this.auditService.getAudits(pageNumber).subscribe({
      next: (audits) => {
        this.audits = audits;
      },
      error: () => {
        this.error = 'Failed to fetch audits.';
        this.audits.items = [];
        this.loadingService.complete();
      },
      complete: () => {
        this.error = null;
        this.loadingService.complete();
      }
    });
  }

  onPageChange(pageNumber: number) {
    this.getAudits(pageNumber);
  }
}
