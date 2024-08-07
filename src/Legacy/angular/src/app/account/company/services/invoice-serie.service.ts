import {Injectable} from "@angular/core";
import {HttpClient, HttpParams} from "@angular/common/http";
import {environment} from "environments/environment";
import {InvoiceSerie} from "../interfaces";
import {Observable} from "rxjs";
import {UserDataService} from "app/common/user-data.service";

@Injectable({
  providedIn: "root"
})
export class InvoiceSerieService {
  private controller: string = "InvoiceSerie";
  private baseUrl: string = environment.applicationUrl + "account";

  constructor(
    private http: HttpClient,
    private userDataService: UserDataService) {
  }

  public index(query: string = "", companyId: string = ""): Observable<InvoiceSerie[]> {
    let params = new HttpParams();
    params = params.append("query", query);
    if (companyId === "") companyId = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    return this.http.get<InvoiceSerie[]>(url, {params});
  }

  public show(id: string, companyId: string = ""): Observable<InvoiceSerie> {
    if (companyId === "") companyId = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.get<InvoiceSerie>(url);
  }

  public create(data: InvoiceSerie, companyId: string = ""): Observable<InvoiceSerie> {
    data.companyId = companyId.trim();
    if (companyId === "") companyId = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    return this.http.post<InvoiceSerie>(url, data);
  }

  public update(id: string, data: InvoiceSerie, companyId: string = ""): Observable<InvoiceSerie> {
    data.companyId = companyId.trim();
    if (companyId === "") companyId = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.put<InvoiceSerie>(url, data);
  }

  public delete(id: string, companyId: string = ""): Observable<InvoiceSerie> {
    if (companyId === "") companyId = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.delete<InvoiceSerie>(url);
  }

}
