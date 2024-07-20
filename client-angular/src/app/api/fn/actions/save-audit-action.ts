/* tslint:disable */
/* eslint-disable */
import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { ApiSaveAuditActionRequest } from '../../models/api-save-audit-action-request';
import { ApiSaveAuditActionResponse } from '../../models/api-save-audit-action-response';

export interface SaveAuditAction$Params {
      body?: ApiSaveAuditActionRequest
}

export function saveAuditAction(http: HttpClient, rootUrl: string, params?: SaveAuditAction$Params, context?: HttpContext): Observable<StrictHttpResponse<ApiSaveAuditActionResponse>> {
  const rb = new RequestBuilder(rootUrl, saveAuditAction.PATH, 'post');
  if (params) {
    rb.body(params.body, 'application/*+json');
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<ApiSaveAuditActionResponse>;
    })
  );
}

saveAuditAction.PATH = '/api/actions';
