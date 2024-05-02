/* tslint:disable */
/* eslint-disable */
import { HttpClient, HttpContext } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { BaseService } from '../base-service';
import { ApiConfiguration } from '../api-configuration';
import { StrictHttpResponse } from '../strict-http-response';

import { getAudit } from '../fn/audits/get-audit';
import { GetAudit$Params } from '../fn/audits/get-audit';
import { GetAuditResponse } from '../models/get-audit-response';
import { listAudits } from '../fn/audits/list-audits';
import { ListAudits$Params } from '../fn/audits/list-audits';
import { ListAuditsResponse } from '../models/list-audits-response';
import { saveAudit } from '../fn/audits/save-audit';
import { SaveAudit$Params } from '../fn/audits/save-audit';
import { SaveAuditResponse } from '../models/save-audit-response';

@Injectable({ providedIn: 'root' })
export class ApiAuditsService extends BaseService {
  constructor(config: ApiConfiguration, http: HttpClient) {
    super(config, http);
  }

  /** Path part for operation `listAudits()` */
  static readonly ListAuditsPath = '/api/audits';

  /**
   * Gets list of audits.
   *
   *
   *
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `listAudits()` instead.
   *
   * This method doesn't expect any request body.
   */
  listAudits$Response(params?: ListAudits$Params, context?: HttpContext): Observable<StrictHttpResponse<ListAuditsResponse>> {
    return listAudits(this.http, this.rootUrl, params, context);
  }

  /**
   * Gets list of audits.
   *
   *
   *
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `listAudits$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  listAudits(params?: ListAudits$Params, context?: HttpContext): Observable<ListAuditsResponse> {
    return this.listAudits$Response(params, context).pipe(
      map((r: StrictHttpResponse<ListAuditsResponse>): ListAuditsResponse => r.body)
    );
  }

  /** Path part for operation `saveAudit()` */
  static readonly SaveAuditPath = '/api/audits';

  /**
   * Creates a new audit.
   *
   *
   *
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `saveAudit()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  saveAudit$Response(params?: SaveAudit$Params, context?: HttpContext): Observable<StrictHttpResponse<SaveAuditResponse>> {
    return saveAudit(this.http, this.rootUrl, params, context);
  }

  /**
   * Creates a new audit.
   *
   *
   *
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `saveAudit$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  saveAudit(params?: SaveAudit$Params, context?: HttpContext): Observable<SaveAuditResponse> {
    return this.saveAudit$Response(params, context).pipe(
      map((r: StrictHttpResponse<SaveAuditResponse>): SaveAuditResponse => r.body)
    );
  }

  /** Path part for operation `getAudit()` */
  static readonly GetAuditPath = '/api/audits/{id}';

  /**
   * Gets audit by ID.
   *
   *
   *
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `getAudit()` instead.
   *
   * This method doesn't expect any request body.
   */
  getAudit$Response(params: GetAudit$Params, context?: HttpContext): Observable<StrictHttpResponse<GetAuditResponse>> {
    return getAudit(this.http, this.rootUrl, params, context);
  }

  /**
   * Gets audit by ID.
   *
   *
   *
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `getAudit$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  getAudit(params: GetAudit$Params, context?: HttpContext): Observable<GetAuditResponse> {
    return this.getAudit$Response(params, context).pipe(
      map((r: StrictHttpResponse<GetAuditResponse>): GetAuditResponse => r.body)
    );
  }

}
