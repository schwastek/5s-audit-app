/* tslint:disable */
/* eslint-disable */
import { HttpClient, HttpContext } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { BaseService } from '../base-service';
import { ApiConfiguration } from '../api-configuration';
import { StrictHttpResponse } from '../strict-http-response';

import { getBadRequest } from '../fn/buggy/get-bad-request';
import { GetBadRequest$Params } from '../fn/buggy/get-bad-request';
import { getNotFound } from '../fn/buggy/get-not-found';
import { GetNotFound$Params } from '../fn/buggy/get-not-found';
import { getServerError } from '../fn/buggy/get-server-error';
import { GetServerError$Params } from '../fn/buggy/get-server-error';
import { getUnauthorised } from '../fn/buggy/get-unauthorised';
import { GetUnauthorised$Params } from '../fn/buggy/get-unauthorised';

@Injectable({ providedIn: 'root' })
export class ApiBuggyService extends BaseService {
  constructor(config: ApiConfiguration, http: HttpClient) {
    super(config, http);
  }

  /** Path part for operation `getNotFound()` */
  static readonly GetNotFoundPath = '/api/errors/notfound';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getNotFound()` instead.
   *
   * This method doesn't expect any request body.
   */
  getNotFound$Response(params?: GetNotFound$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
    return getNotFound(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getNotFound$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  getNotFound(params?: GetNotFound$Params, context?: HttpContext): Observable<void> {
    return this.getNotFound$Response(params, context).pipe(
      map((r: StrictHttpResponse<void>): void => r.body)
    );
  }

  /** Path part for operation `getBadRequest()` */
  static readonly GetBadRequestPath = '/api/errors/badrequest';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getBadRequest()` instead.
   *
   * This method doesn't expect any request body.
   */
  getBadRequest$Response(params?: GetBadRequest$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
    return getBadRequest(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getBadRequest$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  getBadRequest(params?: GetBadRequest$Params, context?: HttpContext): Observable<void> {
    return this.getBadRequest$Response(params, context).pipe(
      map((r: StrictHttpResponse<void>): void => r.body)
    );
  }

  /** Path part for operation `getServerError()` */
  static readonly GetServerErrorPath = '/api/errors/servererror';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getServerError()` instead.
   *
   * This method doesn't expect any request body.
   */
  getServerError$Response(params?: GetServerError$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
    return getServerError(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getServerError$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  getServerError(params?: GetServerError$Params, context?: HttpContext): Observable<void> {
    return this.getServerError$Response(params, context).pipe(
      map((r: StrictHttpResponse<void>): void => r.body)
    );
  }

  /** Path part for operation `getUnauthorised()` */
  static readonly GetUnauthorisedPath = '/api/errors/unauthorised';

  /**
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getUnauthorised()` instead.
   *
   * This method doesn't expect any request body.
   */
  getUnauthorised$Response(params?: GetUnauthorised$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
    return getUnauthorised(this.http, this.rootUrl, params, context);
  }

  /**
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getUnauthorised$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  getUnauthorised(params?: GetUnauthorised$Params, context?: HttpContext): Observable<void> {
    return this.getUnauthorised$Response(params, context).pipe(
      map((r: StrictHttpResponse<void>): void => r.body)
    );
  }

}
