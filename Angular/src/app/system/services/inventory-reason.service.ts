import {Injectable} from '@angular/core';
import {environment} from 'src/environments/environment';
import {HttpClient} from '@angular/common/http';
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

  public index(type: string): Observable<InventoryReason[]> {
    return this.http.get<InventoryReason[]>(`${this.appURL}/Index/${type}`);
  }

  public store(data: InventoryReason): Observable<ResponseData<InventoryReason>> {
    return this.http.post<ResponseData<InventoryReason>>(`${this.appURL}/Store`, data);
  }

  public update(id: number, data: InventoryReason): Observable<ResponseData<InventoryReason>> {
    return this.http.put<ResponseData<InventoryReason>>(`${this.appURL}/Update/${id}`, data);
  }

  public delete(id: number): Observable<ResponseData<InventoryReason>> {
    return this.http.delete<ResponseData<InventoryReason>>(`${this.appURL}/Destroy/${id}`);
  }

}
