/* tslint:disable */
/* eslint-disable */
import { HttpClient, HttpContext } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { BaseService } from '../base-service';
import { ApiConfiguration } from '../api-configuration';
import { StrictHttpResponse } from '../strict-http-response';

import { listQuestions } from '../fn/questions/list-questions';
import { ListQuestions$Params } from '../fn/questions/list-questions';
import { ApiListQuestionsResponse } from '../models/api-list-questions-response';

@Injectable({ providedIn: 'root' })
export class ApiQuestionsService extends BaseService {
  constructor(config: ApiConfiguration, http: HttpClient) {
    super(config, http);
  }

  /** Path part for operation `listQuestions()` */
  static readonly ListQuestionsPath = '/api/questions';

  /**
   * Gets the list of questions.
   *
   *
   *
   * This method provides access to the full `HttpResponse`, allowing access to response headers.
   * To access only the response body, use `listQuestions()` instead.
   *
   * This method doesn't expect any request body.
   */
  listQuestions$Response(params?: ListQuestions$Params, context?: HttpContext): Observable<StrictHttpResponse<ApiListQuestionsResponse>> {
    return listQuestions(this.http, this.rootUrl, params, context);
  }

  /**
   * Gets the list of questions.
   *
   *
   *
   * This method provides access only to the response body.
   * To access the full response (for headers, for example), `listQuestions$Response()` instead.
   *
   * This method doesn't expect any request body.
   */
  listQuestions(params?: ListQuestions$Params, context?: HttpContext): Observable<ApiListQuestionsResponse> {
    return this.listQuestions$Response(params, context).pipe(
      map((r: StrictHttpResponse<ApiListQuestionsResponse>): ApiListQuestionsResponse => r.body)
    );
  }

}
