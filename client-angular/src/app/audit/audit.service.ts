import { Injectable } from '@angular/core';
import { Audit } from './models/audit';
import { NEVER, Observable, of } from 'rxjs';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { PaginatedResult } from './models/pagination';

@Injectable({
  providedIn: 'root'
})
export class AuditService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getAudits(): Observable<PaginatedResult<Audit>> {
    const url = `${this.baseUrl}/api/audits`;
    const paginationParams = new HttpParams()
      .set('pageSize', 5)
      .set('pageNumber', 1);

    return this.http.get<PaginatedResult<Audit>>(url, { params: paginationParams });
  }
}
