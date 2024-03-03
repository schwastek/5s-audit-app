/* tslint:disable */
/* eslint-disable */
import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { LoginRequest } from '../../models/login-request';
import { UserDto } from '../../models/user-dto';

export interface Login$Params {
      body?: LoginRequest
}

export function login(http: HttpClient, rootUrl: string, params?: Login$Params, context?: HttpContext): Observable<StrictHttpResponse<UserDto>> {
  const rb = new RequestBuilder(rootUrl, login.PATH, 'post');
  if (params) {
    rb.body(params.body, 'application/*+json');
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<UserDto>;
    })
  );
}

login.PATH = '/api/account/login';
