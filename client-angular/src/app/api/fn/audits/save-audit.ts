/* tslint:disable */
/* eslint-disable */
import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { SaveAuditRequest } from '../../models/save-audit-request';
import { SaveAuditResponse } from '../../models/save-audit-response';

export interface SaveAudit$Params {
      body?: SaveAuditRequest
}

export function saveAudit(http: HttpClient, rootUrl: string, params?: SaveAudit$Params, context?: HttpContext): Observable<StrictHttpResponse<SaveAuditResponse>> {
  const rb = new RequestBuilder(rootUrl, saveAudit.PATH, 'post');
  if (params) {
    rb.body(params.body, 'application/*+json');
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<SaveAuditResponse>;
    })
  );
}

saveAudit.PATH = '/api/audits';
