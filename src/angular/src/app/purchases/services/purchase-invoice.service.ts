import {HttpClient, HttpParams} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {environment} from "environments/environment";
import {CabeceraCompraDto, PurchaseDto, PurchaseInvoice} from "../interfaces/purchase-invoice";
import {UserDataService} from "../../common/user-data.service";
import {Observable} from "rxjs";

@Injectable({
  providedIn: "root"
})
export class PurchaseInvoiceService {
  private controller: string = "PurchaseInvoice";
  private baseUrl: string = environment.applicationUrl + "purchases";

  constructor(
    private http: HttpClient,
    private userDataService: UserDataService) {
  }

  public index(year: string, month: string): Observable<PurchaseInvoice[]> {
    let params = new HttpParams();
    params = params.append("Year", year);
    params = params.append("Month", month);
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    return this.http.get<PurchaseInvoice[]>(url, {params});
  }

  public show(id: string): Observable<PurchaseDto> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.get<PurchaseDto>(url);
  }

  public create(data: CabeceraCompraDto): Observable<PurchaseInvoice> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    return this.http.post<PurchaseInvoice>(url, data);
  }

  public update(id: string, data: CabeceraCompraDto): Observable<PurchaseInvoice> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.put<PurchaseInvoice>(url, data);
  }

  public delete(id: string): Observable<PurchaseInvoice> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.delete<PurchaseInvoice>(url);
  }

}
