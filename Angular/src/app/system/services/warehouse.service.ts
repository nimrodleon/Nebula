import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {environment} from 'src/environments/environment';
import {Observable} from 'rxjs';
import {Warehouse} from '../interfaces';
import {ResponseData} from '../../global/interfaces';

@Injectable({
  providedIn: 'root'
})
export class WarehouseService {
  private appURL: string = environment.applicationUrl + 'Warehouse';

  constructor(private http: HttpClient) {
  }

  public index(query: string = ''): Observable<Warehouse[]> {
    let params = new HttpParams();
    params = params.append('query', query);
    return this.http.get<Warehouse[]>(`${this.appURL}/Index`, {params: params});
  }

  public store(data: Warehouse): Observable<ResponseData<Warehouse>> {
    return this.http.post<ResponseData<Warehouse>>(`${this.appURL}/Store`, data);
  }

}
