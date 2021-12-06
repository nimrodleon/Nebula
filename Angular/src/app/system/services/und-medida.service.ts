import {Injectable} from '@angular/core';
import {environment} from 'src/environments/environment';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {ResponseData} from '../../global/interfaces';
import {UndMedida} from '../interfaces';

@Injectable({
  providedIn: 'root'
})
export class UndMedidaService {
  private appURL: string = environment.applicationUrl + 'UndMedida';

  constructor(private http: HttpClient) {
  }

  public index(query: string = ''): Observable<UndMedida[]> {
    let params = new HttpParams();
    params = params.append('query', query);
    return this.http.get<UndMedida[]>(`${this.appURL}/Index`, {params: params});
  }

  public show(id: string): Observable<UndMedida> {
    return this.http.get<UndMedida>(`${this.appURL}/Show/${id}`);
  }

  public create(data: UndMedida): Observable<ResponseData<UndMedida>> {
    return this.http.post<ResponseData<UndMedida>>(`${this.appURL}/Create`, data);
  }

  public update(id: string, data: UndMedida): Observable<ResponseData<UndMedida>> {
    return this.http.put<ResponseData<UndMedida>>(`${this.appURL}/Update/${id}`, data);
  }

  public delete(id: string): Observable<ResponseData<UndMedida>> {
    return this.http.delete<ResponseData<UndMedida>>(`${this.appURL}/Delete/${id}`);
  }

}
