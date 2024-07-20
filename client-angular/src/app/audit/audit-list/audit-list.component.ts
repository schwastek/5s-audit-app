import { Component, OnInit } from '@angular/core';
import { AuditService } from '../audit.service';
import { PaginatedResult } from '../../shared/models/pagination';
import { RouterLink } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { PaginationComponent } from '../../shared/components/pagination/pagination.component';
import { AuditListItemDto } from '../models/audit.models';

@Component({
  selector: 'app-audit-list',
  templateUrl: './audit-list.component.html',
  styleUrls: ['./audit-list.component.scss'],
  standalone: true,
  imports: [RouterLink, PaginationComponent]
})
export class AuditListComponent implements OnInit {
  audits: PaginatedResult<AuditListItemDto> | null = null;
  error: string | null = null;
  isLoading: boolean | null = false;

  constructor(private auditService: AuditService) {}

  async ngOnInit() {
    await this.getAudits(1);
  }

  async getAudits(pageNumber: number) {
    try {
      this.isLoading = true;
      this.audits = await firstValueFrom(this.auditService.getAudits(pageNumber));
    } catch (err: unknown) {
      if (err instanceof HttpErrorResponse) {
        this.audits = null;
        this.error = err.message;
      }
      throw err;
    } finally {
      this.isLoading = false;
    }
  }

  async onPageChange(pageNumber: number) {
    await this.getAudits(pageNumber);
  }
}
