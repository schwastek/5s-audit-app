import { Injectable } from '@angular/core';
import { Observable, map, of } from 'rxjs';
import { PaginatedResult } from './models/pagination';
import { Area } from './models/area';
import { ApiAuditsService, ApiQuestionsService } from '../api/services';
import { ListAudits$Params } from '../api/fn/audits/list-audits';
import { AuditListItemDto, SaveAuditRequest } from '../api/models';
import { Nullable } from '../shared/ts-helpers/ts-helpers';
import { isDefined } from '../shared/utilities/utilities';

@Injectable({
  providedIn: 'root'
})
export class AuditService {
  constructor(
    private apiAuditsService: ApiAuditsService,
    private apiQuestionsService: ApiQuestionsService
  ) { }

  getAudits(): Observable<PaginatedResult<AuditListItemDto>> {
    const params: ListAudits$Params = {
      pageSize: 5,
      pageNumber: 1
    };

    const result = this.apiAuditsService.listAudits(params).pipe(
      map((response) => {
        return new PaginatedResult<AuditListItemDto>(response.items, response.metadata);
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

  saveAudit(audit: SaveAuditRequest) {
    return this.apiAuditsService.saveAudit({ body: audit });
  }
}
