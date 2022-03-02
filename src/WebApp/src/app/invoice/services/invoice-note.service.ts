import {Injectable} from '@angular/core';
import {environment} from 'src/environments/environment';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {InvoiceNote, NotaComprobante, VoucherQuery} from '../interfaces';
import {ResponseData} from 'src/app/global/interfaces';

@Injectable({
  providedIn: 'root'
})
export class InvoiceNoteService {
  private appURL: string = environment.applicationUrl + 'InvoiceNote';

  constructor(private http: HttpClient) {
  }

  // listar notas de crédito/débito.
  public index(type: string, query: VoucherQuery): Observable<Array<InvoiceNote>> {
    let params = new HttpParams();
    params = params.append('Year', query.year);
    params = params.append('Month', query.month);
    params = params.append('Query', query.query);
    return this.http.get<Array<InvoiceNote>>(`${this.appURL}/Index/${type}`, {params});
  }

  // cargar información de la nota de crédito/débito.
  public show(id: number): Observable<InvoiceNote> {
    return this.http.get<InvoiceNote>(`${this.appURL}/Show/${id}`);
  }

  // registrar nota de crédito/débito.
  public create(data: NotaComprobante): Observable<ResponseData<NotaComprobante>> {
    return this.http.post<ResponseData<NotaComprobante>>(`${this.appURL}/Create`, data);
  }

  // actualizar registro nota de crédito/débito.
  public update(id: number, data: NotaComprobante): Observable<ResponseData<NotaComprobante>> {
    return this.http.put<ResponseData<NotaComprobante>>(`${this.appURL}/Update/${id}`, data);
  }

}
