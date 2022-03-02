import {Injectable} from '@angular/core';
import {environment} from 'src/environments/environment';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {InventoryReason} from '../interfaces';
import {ResponseData} from '../../global/interfaces';

@Injectable({
  providedIn: 'root'
})
export class InventoryReasonService {
  private appURL: string = environment.applicationUrl + 'InventoryReason';

  constructor(
    private http: HttpClient) {
  }

  public index(type: string, query: string = ''): Observable<InventoryReason[]> {
    let params = new HttpParams();
    params = params.append('query', query);
    return this.http.get<InventoryReason[]>(`${this.appURL}/Index/${type}`, {params: params});
  }

  public show(id: number): Observable<InventoryReason> {
    return this.http.get<InventoryReason>(`${this.appURL}/Show/${id}`);
  }

  public create(data: InventoryReason): Observable<ResponseData<InventoryReason>> {
    return this.http.post<ResponseData<InventoryReason>>(`${this.appURL}/Create`, data);
  }

  public update(id: number, data: InventoryReason): Observable<ResponseData<InventoryReason>> {
    return this.http.put<ResponseData<InventoryReason>>(`${this.appURL}/Update/${id}`, data);
  }

  public delete(id: number): Observable<ResponseData<InventoryReason>> {
    return this.http.delete<ResponseData<InventoryReason>>(`${this.appURL}/Delete/${id}`);
  }

}
