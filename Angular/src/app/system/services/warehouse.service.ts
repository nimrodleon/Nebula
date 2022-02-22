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
    return this.http.get<Warehouse[]>(`${this.appURL}/Index`, {params});
  }

  public show(id: string): Observable<Warehouse> {
    return this.http.get<Warehouse>(`${this.appURL}/Show/${id}`);
  }

  public create(data: Warehouse): Observable<ResponseData<Warehouse>> {
    return this.http.post<ResponseData<Warehouse>>(`${this.appURL}/Create`, data);
  }

  public update(id: string, data: Warehouse): Observable<ResponseData<Warehouse>> {
    return this.http.put<ResponseData<Warehouse>>(`${this.appURL}/Update/${id}`, data);
  }

  public delete(id: string): Observable<ResponseData<Warehouse>> {
    return this.http.delete<ResponseData<Warehouse>>(`${this.appURL}/Delete/${id}`);
  }

}
