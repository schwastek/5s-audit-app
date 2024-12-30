/* tslint:disable */
/* eslint-disable */
import { HttpClient, HttpContext } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { BaseService } from '../base-service';
import { ApiConfiguration } from '../api-configuration';
import { StrictHttpResponse } from '../strict-http-response';

import { deleteAuditAction } from '../fn/actions/delete-audit-action';
import { DeleteAuditAction$Params } from '../fn/actions/delete-audit-action';
import { saveAuditAction } from '../fn/actions/save-audit-action';
import { SaveAuditAction$Params } from '../fn/actions/save-audit-action';
import { ApiSaveAuditActionResponse } from '../models/api-save-audit-action-response';
import { updateAuditAction } from '../fn/actions/update-audit-action';
import { UpdateAuditAction$Params } from '../fn/actions/update-audit-action';

@Injectable({ providedIn: 'root' })
export class ApiActionsService extends BaseService {
  constructor(config: ApiConfiguration, http: HttpClient) {
    super(config, http);
  }

  /** Path part for operation `saveAuditAction()` */
  static readonly SaveAuditActionPath = '/api/actions';

  /**
   * Adds an action to the audit.
   *
   *
   *
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `saveAuditAction()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  saveAuditAction$Response(params?: SaveAuditAction$Params, context?: HttpContext): Observable<StrictHttpResponse<ApiSaveAuditActionResponse>> {
    return saveAuditAction(this.http, this.rootUrl, params, context);
  }

  /**
   * Adds an action to the audit.
   *
   *
   *
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `saveAuditAction$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  saveAuditAction(params?: SaveAuditAction$Params, context?: HttpContext): Observable<ApiSaveAuditActionResponse> {
    return this.saveAuditAction$Response(params, context).pipe(
      map((r: StrictHttpResponse<ApiSaveAuditActionResponse>): ApiSaveAuditActionResponse => r.body)
    );
  }

  /** Path part for operation `updateAuditAction()` */
  static readonly UpdateAuditActionPath = '/api/actions/{auditActionId}';

  /**
   * Updates an action.
   *
   *
   *
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `updateAuditAction()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  updateAuditAction$Response(params: UpdateAuditAction$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
    return updateAuditAction(this.http, this.rootUrl, params, context);
  }

  /**
   * Updates an action.
   *
   *
   *
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `updateAuditAction$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  updateAuditAction(params: UpdateAuditAction$Params, context?: HttpContext): Observable<void> {
    return this.updateAuditAction$Response(params, context).pipe(
      map((r: StrictHttpResponse<void>): void => r.body)
    );
  }

  /** Path part for operation `deleteAuditAction()` */
  static readonly DeleteAuditActionPath = '/api/actions/{auditActionId}';

  /**
   * Deletes an action.
   *
   *
   *
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `deleteAuditAction()` instead.
   *
   * This method doesn't expect any request body.
   */
  deleteAuditAction$Response(params: DeleteAuditAction$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
    return deleteAuditAction(this.http, this.rootUrl, params, context);
  }

  /**
   * Deletes an action.
   *
   *
   *
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `deleteAuditAction$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  deleteAuditAction(params: DeleteAuditAction$Params, context?: HttpContext): Observable<void> {
    return this.deleteAuditAction$Response(params, context).pipe(
      map((r: StrictHttpResponse<void>): void => r.body)
    );
  }

}
