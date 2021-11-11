import {Injectable} from '@angular/core';
import {environment} from 'src/environments/environment';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {ResponseData} from '../../global/interfaces';
import {Invoice} from '../interfaces';
import {Sale} from '../../cashier/interfaces';

@Injectable({
  providedIn: 'root'
})
export class InvoiceService {
  private appURL: string = environment.applicationUrl + 'Invoice';

  constructor(private http: HttpClient) {
  }

  public show(id: number): Observable<Invoice> {
    return this.http.get<Invoice>(`${this.appURL}/Show/${id}`);
  }

  public salePos(id: number, sale: Sale): Observable<ResponseData<Sale>> {
    return this.http.post<ResponseData<Sale>>(`${this.appURL}/SalePos/${id}`, sale);
  }

}
