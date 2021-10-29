import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {CashierDetail} from '../interfaces';

@Injectable({
  providedIn: 'root'
})
export class CashierDetailService {
  private appURL: string = environment.applicationUrl + 'CashierDetail';

  constructor(private http: HttpClient) {
  }

  public index(id: number, query: string): Observable<CashierDetail[]> {
    let params = new HttpParams();
    params = params.append('query', query);
    return this.http.get<CashierDetail[]>(`${this.appURL}/Index/${id}`, {params: params});
  }

}
