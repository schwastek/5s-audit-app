/* tslint:disable */
/* eslint-disable */
import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { ApiRegisterRequest } from '../../models/api-register-request';
import { ApiUserDto } from '../../models/api-user-dto';

export interface Register$Params {
      body?: ApiRegisterRequest
}

export function register(http: HttpClient, rootUrl: string, params?: Register$Params, context?: HttpContext): Observable<StrictHttpResponse<ApiUserDto>> {
  const rb = new RequestBuilder(rootUrl, register.PATH, 'post');
  if (params) {
    rb.body(params.body, 'application/*+json');
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<ApiUserDto>;
    })
  );
}

register.PATH = '/api/account/register';
