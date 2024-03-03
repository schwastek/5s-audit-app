/* tslint:disable */
/* eslint-disable */
import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { ListAuditsResponse } from '../../models/list-audits-response';

export interface ListAudits$Params {
  orderBy?: string;
  pageNumber?: number;
  pageSize?: number;
}

export function listAudits(http: HttpClient, rootUrl: string, params?: ListAudits$Params, context?: HttpContext): Observable<StrictHttpResponse<ListAuditsResponse>> {
  const rb = new RequestBuilder(rootUrl, listAudits.PATH, 'get');
  if (params) {
    rb.query('orderBy', params.orderBy, {});
    rb.query('pageNumber', params.pageNumber, {});
    rb.query('pageSize', params.pageSize, {});
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<ListAuditsResponse>;
    })
  );
}

listAudits.PATH = '/api/audits';
