import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "environments/environment";
import {
  CuentaPorCobrarClienteAnualParam,
  Receivable,
  ReceivableRequestParams,
  ResumenDeuda
} from "../interfaces";
import { UserDataService } from "app/common/user-data.service";
import { Observable } from "rxjs";
import { PaginationResult } from "app/common/interfaces";

@Injectable({
  providedIn: "root"
})
export class ReceivableService {
  private controller: string = "Receivable";
  private baseUrl: string = environment.applicationUrl + "finanzas";

  constructor(
    private http: HttpClient,
    private userDataService: UserDataService) {
  }

  public index(requestParam: ReceivableRequestParams, page: number = 1): Observable<PaginationResult<Receivable>> {
    let params = new HttpParams();
    params = params.append("month", requestParam.month);
    params = params.append("year", requestParam.year);
    params = params.append("status", requestParam.status);
    params = params.append("page", page || 1);
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    return this.http.get<PaginationResult<Receivable>>(url, { params });
  }

  public show(id: string): Observable<Receivable> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.get<Receivable>(url);
  }

  public getReceivablesByContactId(contactId: string, requestParam: ReceivableRequestParams): Observable<Array<Receivable>> {
    let params = new HttpParams();
    params = params.append("month", requestParam.month);
    params = params.append("year", requestParam.year);
    params = params.append("status", requestParam.status);
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/GetReceivablesByContactId/${contactId}`;
    return this.http.get<Array<Receivable>>(url, { params });
  }

  public getCargosCliente(param: CuentaPorCobrarClienteAnualParam): Observable<Array<Receivable>> {
    let params = new HttpParams();
    params = params.append("year", param.year);
    params = params.append("status", param.status);
    params = params.append("contactId", param.contactId);
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/GetCargosCliente`;
    return this.http.get<Array<Receivable>>(url, { params });
  }

  public create(data: Receivable): Observable<Receivable> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    data.companyId = companyId.trim();
    return this.http.post<Receivable>(url, data);
  }

  public update(id: string, data: Receivable): Observable<Receivable> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    data.companyId = companyId.trim();
    return this.http.put<Receivable>(url, data);
  }

  public delete(id: string): Observable<Receivable> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.delete<Receivable>(url);
  }

  public abonos(id: string): Observable<Receivable[]> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/Abonos/${id}`;
    return this.http.get<Receivable[]>(url);
  }

  public totalAbonos(id: string): Observable<number> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/TotalAbonos/${id}`;
    return this.http.get<number>(url);
  }

  public getPendientesPorCobrar(): Observable<ResumenDeuda> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/PendientesPorCobrar`;
    return this.http.get<ResumenDeuda>(url);
  }

}
