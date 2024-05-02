/* tslint:disable */
/* eslint-disable */
import { HttpClient, HttpContext } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { BaseService } from '../base-service';
import { ApiConfiguration } from '../api-configuration';
import { StrictHttpResponse } from '../strict-http-response';

import { getCurrentUser } from '../fn/account/get-current-user';
import { GetCurrentUser$Params } from '../fn/account/get-current-user';
import { login } from '../fn/account/login';
import { Login$Params } from '../fn/account/login';
import { refreshToken } from '../fn/account/refresh-token';
import { RefreshToken$Params } from '../fn/account/refresh-token';
import { register } from '../fn/account/register';
import { Register$Params } from '../fn/account/register';
import { UserDto } from '../models/user-dto';

@Injectable({ providedIn: 'root' })
export class ApiAccountService extends BaseService {
  constructor(config: ApiConfiguration, http: HttpClient) {
    super(config, http);
  }

  /** Path part for operation `login()` */
  static readonly LoginPath = '/api/account/login';

  /**
   * Returns access token.
   *
   *
   *
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `login()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  login$Response(params?: Login$Params, context?: HttpContext): Observable<StrictHttpResponse<UserDto>> {
    return login(this.http, this.rootUrl, params, context);
  }

  /**
   * Returns access token.
   *
   *
   *
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `login$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  login(params?: Login$Params, context?: HttpContext): Observable<UserDto> {
    return this.login$Response(params, context).pipe(
      map((r: StrictHttpResponse<UserDto>): UserDto => r.body)
    );
  }

  /** Path part for operation `register()` */
  static readonly RegisterPath = '/api/account/register';

  /**
   * Registers new user.
   *
   *
   *
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `register()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  register$Response(params?: Register$Params, context?: HttpContext): Observable<StrictHttpResponse<UserDto>> {
    return register(this.http, this.rootUrl, params, context);
  }

  /**
   * Registers new user.
   *
   *
   *
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `register$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  register(params?: Register$Params, context?: HttpContext): Observable<UserDto> {
    return this.register$Response(params, context).pipe(
      map((r: StrictHttpResponse<UserDto>): UserDto => r.body)
    );
  }

  /** Path part for operation `getCurrentUser()` */
  static readonly GetCurrentUserPath = '/api/account';

  /**
   * Gets user information.
   *
   *
   *
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getCurrentUser()` instead.
   *
   * This method doesn't expect any request body.
   */
  getCurrentUser$Response(params?: GetCurrentUser$Params, context?: HttpContext): Observable<StrictHttpResponse<UserDto>> {
    return getCurrentUser(this.http, this.rootUrl, params, context);
  }

  /**
   * Gets user information.
   *
   *
   *
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getCurrentUser$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  getCurrentUser(params?: GetCurrentUser$Params, context?: HttpContext): Observable<UserDto> {
    return this.getCurrentUser$Response(params, context).pipe(
      map((r: StrictHttpResponse<UserDto>): UserDto => r.body)
    );
  }

  /** Path part for operation `refreshToken()` */
  static readonly RefreshTokenPath = '/api/account/refreshToken';

  /**
   * Refreshes access token.
   *
   *
   *
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `refreshToken()` instead.
   *
   * This method doesn't expect any request body.
   */
  refreshToken$Response(params?: RefreshToken$Params, context?: HttpContext): Observable<StrictHttpResponse<UserDto>> {
    return refreshToken(this.http, this.rootUrl, params, context);
  }

  /**
   * Refreshes access token.
   *
   *
   *
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `refreshToken$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  refreshToken(params?: RefreshToken$Params, context?: HttpContext): Observable<UserDto> {
    return this.refreshToken$Response(params, context).pipe(
      map((r: StrictHttpResponse<UserDto>): UserDto => r.body)
    );
  }

}
