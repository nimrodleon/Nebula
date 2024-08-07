import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from "environments/environment";
import { ComprobantesPendientes, InvoiceSale, ResponseInvoiceSale, TicketDto } from "../interfaces";
import { UserDataService } from "../../common/user-data.service";
import { ComprobanteDto } from "../../cashier/quicksale/interfaces";
import { PaginationResult } from "app/common/interfaces";

@Injectable({
  providedIn: "root"
})
export class InvoiceSaleService {
  private controller: string = "InvoiceSale";
  private baseUrl: string = environment.applicationUrl + "sales";

  constructor(
    private http: HttpClient,
    private userDataService: UserDataService) {
  }

  public index(year: string, month: string, query: string, page: number = 1): Observable<PaginationResult<InvoiceSale>> {
    let params = new HttpParams();
    params = params.append("Year", year);
    params = params.append("Month", month);
    params = params.append("Query", query);
    params = params.append("page", page || 1);
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    return this.http.get<PaginationResult<InvoiceSale>>(url, { params });
  }

  public create(data: ComprobanteDto): Observable<any> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    return this.http.post<any>(url, data);
  }

  public show(id: string): Observable<ResponseInvoiceSale> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.get<ResponseInvoiceSale>(url);
  }

  public reenviar(invoiceSaleId: string): Observable<any> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/Reenviar/${invoiceSaleId}`;
    return this.http.patch<any>(url, {});
  }

  public comprobantesPendientes(): Observable<ComprobantesPendientes[]> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/Pendientes`;
    return this.http.get<ComprobantesPendientes[]>(url);
  }

  public anularComprobante(id: string): Observable<any> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/AnularComprobante/${id}`;
    return this.http.patch<any>(url, {});
  }

  public delete(id: string): Observable<InvoiceSale> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.delete<InvoiceSale>(url);
  }

  public getTicket(id: string): Observable<TicketDto> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/Ticket/${id}`;
    return this.http.get<TicketDto>(url);
  }

  public descargarRegistroVentas(year: string, month: string): Observable<Blob> {
    let params = new HttpParams();
    params = params.append("Year", year);
    params = params.append("Month", month);
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/DescargarRegistroVentas`;
    return this.http.get(url, { responseType: "blob", params: params });
  }

  public getXml(invoiceId: string): Observable<Blob> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/GetXml/${invoiceId}`;
    return this.http.get(url, { responseType: "blob" });
  }

}
