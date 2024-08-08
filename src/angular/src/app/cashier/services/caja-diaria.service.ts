import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "environments/environment";
import { AperturaCaja, CajaDiaria, CerrarCaja, QuickSaleConfig } from "../interfaces";
import { UserDataService } from "../../common/user-data.service";
import { Observable } from "rxjs";
import { PaginationResult } from "app/common/interfaces";

@Injectable({
  providedIn: "root"
})
export class CajaDiariaService {
  private controller: string = "CajaDiaria";
  private baseUrl: string = environment.applicationUrl + "cashier";

  constructor(
    private http: HttpClient,
    private userDataService: UserDataService) {
  }

  public index(year: string, month: string, page: number = 1): Observable<PaginationResult<CajaDiaria>> {
    let params = new HttpParams();
    params = params.append("Year", year);
    params = params.append("Month", month);
    params = params.append("page", page || 1);
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    return this.http.get<PaginationResult<CajaDiaria>>(url, { params });
  }

  public show(id: string): Observable<CajaDiaria> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.get<CajaDiaria>(url);
  }

  public create(data: AperturaCaja): Observable<CajaDiaria> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    return this.http.post<CajaDiaria>(url, data);
  }

  public update(id: string, data: CerrarCaja): Observable<CajaDiaria> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.put<CajaDiaria>(url, data);
  }

  public delete(id: string): Observable<CajaDiaria> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.delete<CajaDiaria>(url);
  }

  public cajasAbiertas(): Observable<Array<CajaDiaria>> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/CajasAbiertas`;
    return this.http.get<Array<CajaDiaria>>(url);
  }

  /**
   * Configuración inicial del punto de venta.
   * @param cajaDiariaId identificador de la caja.
   */
  public getQuickSaleConfig(cajaDiariaId: string): Observable<QuickSaleConfig> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/GetQuickSaleConfig/${cajaDiariaId}`;
    return this.http.get<QuickSaleConfig>(url);
  }

}
