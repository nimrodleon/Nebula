import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {ResponseInvoiceSale} from '../interfaces';

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

}
