import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
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

  public index(): Observable<Warehouse[]> {
    return this.http.get<Warehouse[]>(`${this.appURL}/Index`);
  }

}
