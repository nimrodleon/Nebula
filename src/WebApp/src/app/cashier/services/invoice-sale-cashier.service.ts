import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {ResponseData} from 'src/app/global/interfaces';
import {InvoiceSaleDetail} from 'src/app/sales/interfaces';
import {Comprobante, GenerarVenta} from '../interfaces';

@Injectable({
  providedIn: 'root'
})
export class InvoiceSaleCashierService {
  private appURL: string = environment.applicationUrl + 'InvoiceSaleCashier';

  constructor(private http: HttpClient) {
  }

  public generarVenta(id: string, data: GenerarVenta): Observable<ResponseData<Comprobante>> {
    const {comprobante, detallesComprobante} = data;
    return this.http.post<ResponseData<Comprobante>>(`${this.appURL}/GenerarVenta/${id}`, {
      comprobante,
      detallesComprobante
    });
  }

  public productReport(id: string): Observable<Array<InvoiceSaleDetail>> {
    return this.http.get<Array<InvoiceSaleDetail>>(`${this.appURL}/ProductReport/${id}`);
  }

}
