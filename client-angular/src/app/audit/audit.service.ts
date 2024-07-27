import { Injectable } from '@angular/core';
import { Observable, map, of } from 'rxjs';
import { PaginatedResult } from '../shared/models/pagination';
import { Area } from './models/area';
import { ApiActionsService, ApiAuditsService, ApiQuestionsService } from '../api/services';
import { ListAudits$Params } from '../api/fn/audits/list-audits';
import { ApiSaveAuditActionRequest, ApiSaveAuditRequest } from '../api/models';
import { Nullable } from '../shared/utilities/ts-helpers';
import { isDefined } from '../shared/utilities/utilities';
import { AuditListItemDto } from './models/audit.models';
import { formatAsPercentage, formatDate, getShortGuid } from '../shared/utilities/formatters';

@Injectable({
  providedIn: 'root'
})
export class AuditService {
  constructor(
    private apiAuditsService: ApiAuditsService,
    private apiQuestionsService: ApiQuestionsService,
    private apiActionService: ApiActionsService
  ) { }

  getAudits(pageNumber?: number | null): Observable<PaginatedResult<AuditListItemDto>> {
    const params: ListAudits$Params = {
      pageSize: 10,
      pageNumber: pageNumber ?? 1
    };

    const result = this.apiAuditsService.listAudits(params).pipe(
      map((response) => {
        const items = response.items.map((item) => {
          return {
            ...item,
            auditIdShort: getShortGuid(item.auditId),
            startDateFormatted: formatDate(item.startDate),
            scorePercentage: formatAsPercentage(item.score)
          } as AuditListItemDto;
        });

        return new PaginatedResult<AuditListItemDto>(items, response.metadata);
      })
    );

    return result;
  }

  getAudit(id: Nullable<string>) {
    if (!isDefined(id)) return of(null);

    return this.apiAuditsService.getAudit({ id: id! }).pipe(
      map((response) => response.audit)
    );
  }

  getAreas(): Observable<Area[]> {
    const areas = [
      { id: 1, key: 'assembly',  value: 'Assembly' },
      { id: 2, key: 'packing',  value: 'Packing' },
      { id: 3, key: 'shipping',   value: 'Shipping' },
      { id: 4, key: 'storage', value: 'Storage' }
    ];

    return of(areas);
  }

  getQuestions() {
    return this.apiQuestionsService.listQuestions().pipe(
      map((response) => response.questions)
    );
  }

  saveAudit(audit: ApiSaveAuditRequest) {
    return this.apiAuditsService.saveAudit({ body: audit });
  }

  saveAction(auditAction: ApiSaveAuditActionRequest) {
    return this.apiActionService.saveAuditAction({ body: auditAction });
  }
}
