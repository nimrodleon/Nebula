import {Injectable} from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import {Observable} from "rxjs";
import {environment} from "environments/environment";
import {CashierDetail, ResumenCajaDto} from "../interfaces";
import {PaginationResult} from "app/common/interfaces";

@Injectable({
  providedIn: "root"
})
export class CashierDetailService {
  private controller: string = "CashierDetail";
  private baseUrl: string = environment.applicationUrl + "cashier";

  constructor(
    private http: HttpClient,) {
  }

  public index(id: string, query: string = "", page: number = 1): Observable<PaginationResult<CashierDetail>> {
    let params = new HttpParams();
    params = params.append("query", query);
    params = params.append("page", page || 1);
    const url: string = `${this.baseUrl}/${this.controller}/${id}`;
    return this.http.get<PaginationResult<CashierDetail>>(url, {params});
  }

  public create(data: CashierDetail): Observable<CashierDetail> {
    const url: string = `${this.baseUrl}/${this.controller}`;
    return this.http.post<CashierDetail>(url, data);
  }

  public delete(id: string): Observable<CashierDetail> {
    const url: string = `${this.baseUrl}/${this.controller}/${id}`;
    return this.http.delete<CashierDetail>(url);
  }

  public countDocuments(id: string): Observable<number> {
    const url: string = `${this.baseUrl}/${this.controller}/CountDocuments/${id}`;
    return this.http.get<number>(url);
  }

  public getResumenCajaDto(cajaDiariaId: string): Observable<ResumenCajaDto> {
    const url: string = `${this.baseUrl}/${this.controller}/ResumenCaja/${cajaDiariaId}`;
    return this.http.get<ResumenCajaDto>(url);
  }

}
