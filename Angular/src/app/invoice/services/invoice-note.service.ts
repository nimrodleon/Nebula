import {Injectable} from '@angular/core';
import {environment} from 'src/environments/environment';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {InvoiceNote, NotaComprobante} from '../interfaces';
import {ResponseData} from '../../global/interfaces';

@Injectable({
  providedIn: 'root'
})
export class InvoiceNoteService {
  private appURL: string = environment.applicationUrl + 'InvoiceNote';

  constructor(private http: HttpClient) {
  }

  // cargar información de la nota de crédito/débito.
  public show(id: number): Observable<InvoiceNote> {
    return this.http.get<InvoiceNote>(`${this.appURL}/Show/${id}`);
  }

  // registrar nota de crédito/débito.
  public create(data: NotaComprobante): Observable<ResponseData<NotaComprobante>> {
    return this.http.post<ResponseData<NotaComprobante>>(`${this.appURL}/Create`, data);
  }

}
