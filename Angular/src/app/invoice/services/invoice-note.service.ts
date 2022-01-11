import {Injectable} from '@angular/core';
import {environment} from 'src/environments/environment';
import {HttpClient} from '@angular/common/http';
import {NotaComprobante} from '../interfaces';
import {Observable} from 'rxjs';
import {ResponseData} from '../../global/interfaces';

@Injectable({
  providedIn: 'root'
})
export class InvoiceNoteService {
  private appURL: string = environment.applicationUrl + 'InvoiceNote';

  constructor(private http: HttpClient) {
  }

  // registrar nota de crédito/débito.
  public create(data: NotaComprobante): Observable<ResponseData<NotaComprobante>> {
    return this.http.post<ResponseData<NotaComprobante>>(`${this.appURL}/Create`, data);
  }

}
