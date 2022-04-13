import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {CashierDetail} from '../interfaces';
import {ResponseData} from '../../global/interfaces';

@Injectable({
  providedIn: 'root'
})
export class CashierDetailService {
  private appURL: string = environment.applicationUrl + 'CashierDetail';

  constructor(private http: HttpClient) {
  }

  public index(id: string, query: string = ''): Observable<CashierDetail[]> {
    let params = new HttpParams();
    params = params.append('query', query);
    return this.http.get<CashierDetail[]>(`${this.appURL}/Index/${id}`, {params});
  }

  public create(data: CashierDetail): Observable<ResponseData<CashierDetail>> {
    return this.http.post<ResponseData<CashierDetail>>(`${this.appURL}/Create`, data);
  }

}
