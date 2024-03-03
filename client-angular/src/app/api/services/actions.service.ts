/* tslint:disable */
/* eslint-disable */
import { HttpClient, HttpContext } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { BaseService } from '../base-service';
import { ApiConfiguration } from '../api-configuration';
import { StrictHttpResponse } from '../strict-http-response';

import { deleteAction } from '../fn/actions/delete-action';
import { DeleteAction$Params } from '../fn/actions/delete-action';
import { saveAuditAction } from '../fn/actions/save-audit-action';
import { SaveAuditAction$Params } from '../fn/actions/save-audit-action';
import { SaveAuditActionResponse } from '../models/save-audit-action-response';
import { updateAction } from '../fn/actions/update-action';
import { UpdateAction$Params } from '../fn/actions/update-action';

@Injectable({ providedIn: 'root' })
export class ActionsService extends BaseService {
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
  saveAuditAction$Response(params?: SaveAuditAction$Params, context?: HttpContext): Observable<StrictHttpResponse<SaveAuditActionResponse>> {
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
  saveAuditAction(params?: SaveAuditAction$Params, context?: HttpContext): Observable<SaveAuditActionResponse> {
    return this.saveAuditAction$Response(params, context).pipe(
      map((r: StrictHttpResponse<SaveAuditActionResponse>): SaveAuditActionResponse => r.body)
    );
  }

  /** Path part for operation `updateAction()` */
  static readonly UpdateActionPath = '/api/actions/{actionId}';

  /**
   * Updates an action.
   *
   *
   *
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `updateAction()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  updateAction$Response(params: UpdateAction$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
    return updateAction(this.http, this.rootUrl, params, context);
  }

  /**
   * Updates an action.
   *
   *
   *
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `updateAction$Response()` instead.
   *
   * This method sends `application/*+json` and handles request body of type `application/*+json`.
   */
  updateAction(params: UpdateAction$Params, context?: HttpContext): Observable<void> {
    return this.updateAction$Response(params, context).pipe(
      map((r: StrictHttpResponse<void>): void => r.body)
    );
  }

  /** Path part for operation `deleteAction()` */
  static readonly DeleteActionPath = '/api/actions/{actionId}';

  /**
   * Deletes an action.
   *
   *
   *
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `deleteAction()` instead.
   *
   * This method doesn't expect any request body.
   */
  deleteAction$Response(params: DeleteAction$Params, context?: HttpContext): Observable<StrictHttpResponse<void>> {
    return deleteAction(this.http, this.rootUrl, params, context);
  }

  /**
   * Deletes an action.
   *
   *
   *
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `deleteAction$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  deleteAction(params: DeleteAction$Params, context?: HttpContext): Observable<void> {
    return this.deleteAction$Response(params, context).pipe(
      map((r: StrictHttpResponse<void>): void => r.body)
    );
  }

}
