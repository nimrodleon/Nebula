import {HttpClient} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {environment} from "environments/environment";
import {ItemCompraDto, PurchaseInvoiceDetail} from "../interfaces/purchase-invoice";
import {UserDataService} from "../../common/user-data.service";
import {Observable} from "rxjs";

@Injectable({
  providedIn: "root"
})
export class PurchaseInvoiceDetailService {
  private controller: string = "PurchaseInvoiceDetail";
  private baseUrl: string = environment.applicationUrl + "purchases";

  constructor(
    private http: HttpClient,
    private userDataService: UserDataService) {
  }

  public create(purchaseInvoiceId: string, data: ItemCompraDto): Observable<PurchaseInvoiceDetail> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${purchaseInvoiceId}`;
    return this.http.post<PurchaseInvoiceDetail>(url, data);
  }

  public update(id: string, data: ItemCompraDto): Observable<PurchaseInvoiceDetail> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.put<PurchaseInvoiceDetail>(url, data);
  }

  public delete(id: string): Observable<PurchaseInvoiceDetail> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.delete<PurchaseInvoiceDetail>(url);
  }

}
