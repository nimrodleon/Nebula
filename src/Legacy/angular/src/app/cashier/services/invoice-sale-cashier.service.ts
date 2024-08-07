import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {environment} from "environments/environment";
import {InvoiceSaleDetail} from "app/sales/interfaces";
import {UserDataService} from "../../common/user-data.service";
import {ComprobanteDto, ResponseCobrarModal} from "../quicksale/interfaces";

@Injectable({
  providedIn: "root"
})
export class InvoiceSaleCashierService {
  private controller: string = "InvoiceSaleCashier";
  private baseUrl: string = environment.applicationUrl + "cashier";

  constructor(
    private http: HttpClient,
    private userDataService: UserDataService) {
  }

  public generarVenta(comprobanteDto: ComprobanteDto): Observable<ResponseCobrarModal> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/GenerarVenta`;
    return this.http.post<ResponseCobrarModal>(url, comprobanteDto);
  }

  public productReport(id: string): Observable<InvoiceSaleDetail[]> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/ProductReport/${id}`;
    return this.http.get<InvoiceSaleDetail[]>(url);
  }

}
