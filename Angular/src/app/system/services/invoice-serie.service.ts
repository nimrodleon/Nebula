import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {environment} from 'src/environments/environment';
import {Observable} from 'rxjs';
import {InvoiceSerie} from '../interfaces/invoice-serie';
import {ResponseData} from '../../global/interfaces';

@Injectable({
  providedIn: 'root'
})
export class InvoiceSerieService {
  private appURL: string = environment.applicationUrl + 'InvoiceSerie';

  constructor(private http: HttpClient) {
  }

  public index(query: string): Observable<InvoiceSerie[]> {
    let params = new HttpParams();
    params = params.append('query', query);
    return this.http.get<InvoiceSerie[]>(`${this.appURL}/Index`, {params: params});
  }

  public show(id: string): Observable<InvoiceSerie> {
    return this.http.get<InvoiceSerie>(`${this.appURL}/Show/${id}`);
  }

  public create(data: InvoiceSerie): Observable<ResponseData<InvoiceSerie>> {
    return this.http.post<ResponseData<InvoiceSerie>>(`${this.appURL}/Create`, data);
  }

  public update(id: string, data: InvoiceSerie): Observable<ResponseData<InvoiceSerie>> {
    return this.http.put<ResponseData<InvoiceSerie>>(`${this.appURL}/Update/${id}`, data);
  }

  public delete(id: string): Observable<ResponseData<InvoiceSerie>> {
    return this.http.delete<ResponseData<InvoiceSerie>>(`${this.appURL}/Delete/${id}`);
  }

}
