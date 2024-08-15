import { Component, OnInit } from '@angular/core';
import { AuditService } from '../audit.service';
import { PaginatedResult } from '../../shared/models/pagination';
import { RouterLink } from '@angular/router';
import { BehaviorSubject, catchError, finalize, Observable, switchMap, tap } from 'rxjs';
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
  public audits$!: Observable<PaginatedResult<AuditListItemDto>>;

  private currentPage = new BehaviorSubject<number>(1);
  public currentPage$ = this.currentPage.asObservable();

  public error: string | null = null;
  public isLoading = true;

  constructor(
    private auditService: AuditService
  ) {}

  ngOnInit() {
    this.getAudits();
  }

  private getAudits() {
    // Get audits when page changes.
    this.audits$ = this.currentPage$.pipe(
      tap(() => {
        this.isLoading = true;
      }),
      switchMap((pageNumber) => {
        return this.auditService.getAudits(pageNumber).pipe(
          catchError((error) => {
            this.error = 'Failed to fetch audits.';
            throw error;
          }),
          finalize(() => {
            this.isLoading = false;
          })
        );
      })
    );
  }

  onPageChange(pageNumber: number) {
    this.currentPage.next(pageNumber);
  }
}
