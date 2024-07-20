/* tslint:disable */
/* eslint-disable */
import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { ApiSaveAuditRequest } from '../../models/api-save-audit-request';
import { ApiSaveAuditResponse } from '../../models/api-save-audit-response';

export interface SaveAudit$Params {
      body?: ApiSaveAuditRequest
}

export function saveAudit(http: HttpClient, rootUrl: string, params?: SaveAudit$Params, context?: HttpContext): Observable<StrictHttpResponse<ApiSaveAuditResponse>> {
  const rb = new RequestBuilder(rootUrl, saveAudit.PATH, 'post');
  if (params) {
    rb.body(params.body, 'application/*+json');
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<ApiSaveAuditResponse>;
    })
  );
}

saveAudit.PATH = '/api/audits';
