/* tslint:disable */
/* eslint-disable */
import { HttpClient, HttpContext, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { StrictHttpResponse } from '../../strict-http-response';
import { RequestBuilder } from '../../request-builder';

import { ListQuestionsResponse } from '../../models/list-questions-response';

export interface ListQuestions$Params {
}

export function listQuestions(http: HttpClient, rootUrl: string, params?: ListQuestions$Params, context?: HttpContext): Observable<StrictHttpResponse<ListQuestionsResponse>> {
  const rb = new RequestBuilder(rootUrl, listQuestions.PATH, 'get');
  if (params) {
  }

  return http.request(
    rb.build({ responseType: 'json', accept: 'application/json', context })
  ).pipe(
    filter((r: any): r is HttpResponse<any> => r instanceof HttpResponse),
    map((r: HttpResponse<any>) => {
      return r as StrictHttpResponse<ListQuestionsResponse>;
    })
  );
}

listQuestions.PATH = '/api/questions';
