import {Injectable} from '@angular/core';
import {environment} from 'src/environments/environment';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {ResponseData} from 'src/app/global/interfaces';
import {InvoiceSale, VoucherQuery} from '../interfaces';
import {Sale} from 'src/app/cashier/interfaces';

@Injectable({
  providedIn: 'root'
})
export class InvoiceSaleService {
  private appURL: string = environment.applicationUrl + 'InvoiceSale';

  constructor(
    private http: HttpClient) {
  }

  public index(type: string, query: VoucherQuery): Observable<InvoiceSale[]> {
    let params = new HttpParams();
    params = params.append('Year', query.year);
    params = params.append('Month', query.month);
    params = params.append('Query', query.query);
    return this.http.get<InvoiceSale[]>(`${this.appURL}/Index/${type}`, {params});
  }

  // obtener registro del comprobante.
  public show(id: number): Observable<InvoiceSale> {
    return this.http.get<InvoiceSale>(`${this.appURL}/Show/${id}`);
  }

  // registrar venta rápida.
  public createQuickSale(id: string, data: Sale): Observable<ResponseData<Sale>> {
    return this.http.post<ResponseData<Sale>>(`${this.appURL}/CreateQuickSale/${id}`, data);
  }

}
