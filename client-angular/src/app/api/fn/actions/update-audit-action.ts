/* tslint:disable */
/* eslint-disable */
import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { ApiUpdateAuditActionRequest } from '../../models/api-update-audit-action-request';

export interface UpdateAuditAction$Params {
  auditActionId: string;
      body?: ApiUpdateAuditActionRequest
}

export function updateAuditAction(http: HttpClient, rootUrl: string, params: UpdateAuditAction$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
  const rb = new RequestBuilder(rootUrl, updateAuditAction.PATH, 'put');
  if (params) {
    rb.path('auditActionId', params.auditActionId, {});
    rb.body(params.body, 'application/*+json');
  }

  return http.request(
    rb.build({ responseType: 'text', accept: '*/*', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return (r as HttpResponse<any>).clone({ body: undefined }) as StrictHttpResponse<void>;
    })
  );
}

updateAuditAction.PATH = '/api/actions/{auditActionId}';
