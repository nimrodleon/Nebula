import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {environment} from 'src/environments/environment';
import {Observable} from 'rxjs';
import {Warehouse} from '../interfaces';

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

}
