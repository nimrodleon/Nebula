import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {Caja} from '../interfaces';
import {ResponseData} from '../../global/interfaces';

@Injectable({
  providedIn: 'root'
})
export class CajaService {
  private appURL: string = environment.applicationUrl + 'Caja';

  constructor(private http: HttpClient) {
  }

  public index(): Observable<Caja[]> {
    return this.http.get<Caja[]>(`${this.appURL}/Index`);
  }

  public show(id: string): Observable<Caja> {
    return this.http.get<Caja>(`${this.appURL}/Show/${id}`);
  }

  public store(data: Caja): Observable<ResponseData<Caja>> {
    return this.http.post<ResponseData<Caja>>(`${this.appURL}/Store`, data);
  }

  public update(id: string, data: Caja): Observable<ResponseData<Caja>> {
    return this.http.put<ResponseData<Caja>>(`${this.appURL}/Update/${id}`, data);
  }

  public destroy(id: string): Observable<ResponseData<Caja>> {
    return this.http.delete<ResponseData<Caja>>(`${this.appURL}/Destroy/${id}`);
  }
}
