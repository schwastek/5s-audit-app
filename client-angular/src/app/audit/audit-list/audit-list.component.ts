import { Component, OnInit } from '@angular/core';
import { AuditService } from '../audit.service';
import { PaginatedResult } from '../models/pagination';
import { NgFor } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AuditListItemDto } from '../../api/models';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-audit-list',
  templateUrl: './audit-list.component.html',
  styleUrls: ['./audit-list.component.scss'],
  standalone: true,
  imports: [NgFor, RouterLink]
})
export class AuditListComponent implements OnInit {
  public audits = new PaginatedResult<AuditListItemDto>();

  constructor(private auditService: AuditService) {}

  async ngOnInit() {
    this.audits = await lastValueFrom(this.auditService.getAudits());
  }
}
