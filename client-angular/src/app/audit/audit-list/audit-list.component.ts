import { Component, OnInit } from '@angular/core';
import { AuditService } from '../audit.service';
import { PaginatedResult } from '../../shared/models/pagination';
import { RouterLink } from '@angular/router';
import { PaginationComponent } from '../../shared/components/pagination/pagination.component';
import { AuditListItemDto } from '../models/audit.models';
import { AsyncPipe } from '@angular/common';
import { LoadingService } from '../../shared/loading/loading.service';
import { LoadingComponent } from '../../shared/loading/loading.component';

@Component({
  selector: 'app-audit-list',
  templateUrl: './audit-list.component.html',
  standalone: true,
  imports: [
    AsyncPipe,
    RouterLink,
    PaginationComponent,
    LoadingComponent
  ],
  providers: [LoadingService]
})
export class AuditListComponent implements OnInit {
  public audits = new PaginatedResult<AuditListItemDto>();
  public error: string | null = null;
  public isLoading = this.loadingService.isLoading;

  constructor(
    private auditService: AuditService,
    private loadingService: LoadingService
  ) {}

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
