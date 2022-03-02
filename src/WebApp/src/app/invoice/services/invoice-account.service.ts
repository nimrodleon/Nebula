import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {InvoiceAccount, VoucherQuery} from '../interfaces';

@Injectable({
  providedIn: 'root'
})
export class InvoiceAccountService {
  private appURL: string = environment.applicationUrl + 'InvoiceAccount';

  constructor(private http: HttpClient) {
  }

  // listar cuentas pendientes de cobro/pago.
  public index(type: string, query: VoucherQuery): Observable<Array<InvoiceAccount>> {
    let params = new HttpParams();
    params = params.append('Year', query.year);
    params = params.append('Month', query.month);
    params = params.append('Query', query.query);
    return this.http.get<Array<InvoiceAccount>>(`${this.appURL}/Index/${type}`, {params});
  }

}
