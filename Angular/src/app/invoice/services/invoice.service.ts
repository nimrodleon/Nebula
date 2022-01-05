import {Injectable} from '@angular/core';
import {environment} from 'src/environments/environment';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {ResponseData} from '../../global/interfaces';
import {Comprobante, Invoice, VoucherQuery} from '../interfaces';
import {Sale} from '../../cashier/interfaces';

@Injectable({
  providedIn: 'root'
})
export class InvoiceService {
  private appURL: string = environment.applicationUrl + 'Invoice';

  constructor(
    private http: HttpClient) {
  }

  public index(type: string, query: VoucherQuery): Observable<Invoice[]> {
    let params = new HttpParams();
    params = params.append('Year', query.year);
    params = params.append('Month', query.month);
    params = params.append('Query', query.query);
    return this.http.get<Invoice[]>(`${this.appURL}/Index/${type}`, {params: params});
  }

  // obtener registro del comprobante.
  public show(id: number): Observable<Invoice> {
    return this.http.get<Invoice>(`${this.appURL}/Show/${id}`);
  }

  // registrar comprobante de venta.
  public createSale(id: number, data: Comprobante): Observable<ResponseData<Comprobante>> {
    return this.http.post<ResponseData<Comprobante>>(`${this.appURL}/CreateSale/${id}`, data);
  }

  // registrar venta rápida.
  public createQuickSale(id: number, data: Sale): Observable<ResponseData<Sale>> {
    return this.http.post<ResponseData<Sale>>(`${this.appURL}/CreateQuickSale/${id}`, data);
  }

  // registrar comprobante de compra.
  public createPurchase(data: Comprobante): Observable<ResponseData<Comprobante>> {
    return this.http.post<ResponseData<Comprobante>>(`${this.appURL}/CreatePurchase`, data);
  }

  // actualizar comprobante de compra.
  public UpdatePurchase(data: Comprobante): Observable<ResponseData<Comprobante>> {
    return this.http.put<ResponseData<Comprobante>>(`${this.appURL}/UpdatePurchase`, data);
  }

}
