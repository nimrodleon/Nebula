import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { PaginationResult } from 'app/common/interfaces';
import { environment } from 'environments/environment';
import { PagoSuscripcion } from '../interfaces';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PagoSuscripcionService {
  private http: HttpClient = inject(HttpClient);
  private controller: string = "PagoSuscripcion";
  private baseUrl: string = environment.applicationUrl + "account";

  public index(userId: string, year: string, query: string = "", page: number = 1): Observable<PaginationResult<PagoSuscripcion>> {
    let params = new HttpParams();
    params = params.append("year", year);
    params = params.append("query", query);
    params = params.append("page", page);
    const url: string = `${this.baseUrl}/${this.controller}/${userId}`;
    return this.http.get<PaginationResult<PagoSuscripcion>>(url, { params });
  }

  public create(data: PagoSuscripcion): Observable<PagoSuscripcion> {
    const url: string = `${this.baseUrl}/${this.controller}`;
    return this.http.post<PagoSuscripcion>(url, data);
  }

  public update(id: string, data: PagoSuscripcion): Observable<PagoSuscripcion> {
    const url: string = `${this.baseUrl}/${this.controller}/${id}`;
    return this.http.put<PagoSuscripcion>(url, data);
  }

  public delete(id: string): Observable<PagoSuscripcion> {
    const url: string = `${this.baseUrl}/${this.controller}/${id}`;
    return this.http.delete<PagoSuscripcion>(url);
  }

}
