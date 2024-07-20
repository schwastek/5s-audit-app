/* tslint:disable */
/* eslint-disable */
import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { ApiUpdateAuditActionRequest } from '../../models/api-update-audit-action-request';

export interface UpdateAction$Params {
  actionId: string;
      body?: ApiUpdateAuditActionRequest
}

export function updateAction(http: HttpClient, rootUrl: string, params: UpdateAction$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
  const rb = new RequestBuilder(rootUrl, updateAction.PATH, 'put');
  if (params) {
    rb.path('actionId', params.actionId, {});
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

updateAction.PATH = '/api/actions/{actionId}';
