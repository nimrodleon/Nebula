import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {InvoiceSale, ResponseInvoiceSale} from '../interfaces';
import {ResponseData} from 'src/app/global/interfaces';

@Injectable({
  providedIn: 'root'
})
export class InvoiceSaleService {
  private appURL: string = environment.applicationUrl + 'InvoiceSale';

  constructor(private http: HttpClient) {
  }

  public show(id: string): Observable<ResponseInvoiceSale> {
    return this.http.get<ResponseInvoiceSale>(`${this.appURL}/Show/${id}`);
  }

  public delete(id: string): Observable<ResponseData<InvoiceSale>> {
    return this.http.delete<ResponseData<InvoiceSale>>(`${this.appURL}/Delete/${id}`);
  }
}
