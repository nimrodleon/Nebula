import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from "environments/environment";
import { CashierDetail } from "app/cashier/interfaces";
import { InvoiceSale } from "app/sales/interfaces";
import { Contact, IContribuyente } from "../interfaces";
import { UserDataService } from "../../common/user-data.service";
import { catchError } from "rxjs/operators";
import { PaginationResult, toastError } from "../../common/interfaces";

@Injectable({
  providedIn: "root"
})
export class ContactService {
  private controller: string = "Contact";
  private baseUrl: string = environment.applicationUrl + "contacts";

  constructor(
    private http: HttpClient,
    private userDataService: UserDataService) {
  }

  public index(query: string, page: number = 1): Observable<PaginationResult<Contact>> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    let params = new HttpParams();
    params = params.append("query", query || "");
    params = params.append("page", page || 1);
    return this.http.get<PaginationResult<Contact>>(url, { params });
  }

  public show(id: string, companyId: string = ""): Observable<Contact> {
    if (companyId === "") companyId = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.get<Contact>(url);
  }

  public create(data: Contact): Observable<Contact> {
    const companyId: string = this.userDataService.companyId;
    data.companyId = companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    return this.http.post<Contact>(url, data)
      .pipe(catchError(err => {
        toastError(err.error);
        throw err;
      }));
  }

  public update(id: string, data: Contact): Observable<Contact> {
    const companyId: string = this.userDataService.companyId;
    data.companyId = companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.put<Contact>(url, data)
      .pipe(catchError(err => {
        toastError(err.error);
        throw err;
      }));
  }

  public delete(id: string): Observable<Contact> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.delete<Contact>(url);
  }

  public entradaSalida(id: string, month: string, year: string): Observable<CashierDetail[]> {
    let params = new HttpParams();
    params = params.append("month", month);
    params = params.append("year", year);
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/EntradaSalida/${id}`;
    return this.http.get<CashierDetail[]>(url, { params });
  }

  public invoiceSale(id: string, month: string, year: string): Observable<InvoiceSale[]> {
    let params = new HttpParams();
    params = params.append("month", month);
    params = params.append("year", year);
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/InvoiceSale/${id}`;
    return this.http.get<InvoiceSale[]>(url, { params });
  }

  public getContribuyente(doc: string): Observable<IContribuyente> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/Contribuyente/${doc}`;
    return this.http.get<IContribuyente>(url);
  }

}
