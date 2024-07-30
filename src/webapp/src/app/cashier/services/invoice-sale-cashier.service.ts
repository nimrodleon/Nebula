import {Injectable} from "@angular/core";
import { HttpClient } from "@angular/common/http";
import {Observable} from "rxjs";
import {environment} from "environments/environment";
import {InvoiceSaleDetail} from "app/sales/interfaces";
import {ComprobanteDto, ResponseCobrarModal} from "../quicksale/interfaces";

@Injectable({
  providedIn: "root"
})
export class InvoiceSaleCashierService {
  private controller: string = "InvoiceSaleCashier";
  private baseUrl: string = environment.applicationUrl + "cashier";

  constructor(
    private http: HttpClient) {
  }

  public generarVenta(comprobanteDto: ComprobanteDto): Observable<ResponseCobrarModal> {
    const url: string = `${this.baseUrl}/${this.controller}/GenerarVenta`;
    return this.http.post<ResponseCobrarModal>(url, comprobanteDto);
  }

  public productReport(id: string): Observable<InvoiceSaleDetail[]> {
    const url: string = `${this.baseUrl}/${this.controller}/ProductReport/${id}`;
    return this.http.get<InvoiceSaleDetail[]>(url);
  }

}
