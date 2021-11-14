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

  public show(id: number): Observable<Invoice> {
    return this.http.get<Invoice>(`${this.appURL}/Show/${id}`);
  }

  public store(data: Comprobante): Observable<ResponseData<Comprobante>> {
    return this.http.post<ResponseData<Comprobante>>(`${this.appURL}/Store`, data);
  }

  public salePos(id: number, sale: Sale): Observable<ResponseData<Sale>> {
    return this.http.post<ResponseData<Sale>>(`${this.appURL}/SalePos/${id}`, sale);
  }

}
