/* tslint:disable */
/* eslint-disable */
import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { ApiGetAuditResponse } from '../../models/api-get-audit-response';

export interface GetAudit$Params {
  id: string;
}

export function getAudit(http: HttpClient, rootUrl: string, params: GetAudit$Params, context?: HttpContext): Observable<StrictHttpResponse<ApiGetAuditResponse>> {
  const rb = new RequestBuilder(rootUrl, getAudit.PATH, 'get');
  if (params) {
    rb.path('id', params.id, {});
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<ApiGetAuditResponse>;
    })
  );
}

getAudit.PATH = '/api/audits/{id}';
