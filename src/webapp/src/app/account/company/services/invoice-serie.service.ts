import {Injectable} from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import {environment} from "environments/environment";
import {InvoiceSerie} from "../interfaces";
import {Observable} from "rxjs";

@Injectable({
  providedIn: "root"
})
export class InvoiceSerieService {
  private controller: string = "InvoiceSerie";
  private baseUrl: string = environment.applicationUrl + "account";

  constructor(
    private http: HttpClient,) {
  }

  public index(query: string = ""): Observable<InvoiceSerie[]> {
    let params = new HttpParams();
    params = params.append("query", query);
    const url: string = `${this.baseUrl}/${this.controller}`;
    return this.http.get<InvoiceSerie[]>(url, {params});
  }

  public show(id: string): Observable<InvoiceSerie> {
    const url: string = `${this.baseUrl}/${this.controller}/${id}`;
    return this.http.get<InvoiceSerie>(url);
  }

  public create(data: InvoiceSerie): Observable<InvoiceSerie> {
    const url: string = `${this.baseUrl}/${this.controller}`;
    return this.http.post<InvoiceSerie>(url, data);
  }

  public update(id: string, data: InvoiceSerie): Observable<InvoiceSerie> {
    const url: string = `${this.baseUrl}/${this.controller}/${id}`;
    return this.http.put<InvoiceSerie>(url, data);
  }

  public delete(id: string): Observable<InvoiceSerie> {
    const url: string = `${this.baseUrl}/${this.controller}/${id}`;
    return this.http.delete<InvoiceSerie>(url);
  }

}
