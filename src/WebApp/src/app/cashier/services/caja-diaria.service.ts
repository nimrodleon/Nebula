import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {AperturaCaja, CajaDiaria, CerrarCaja} from '../interfaces';
import {ResponseData} from 'src/app/global/interfaces';

@Injectable({
  providedIn: 'root'
})
export class CajaDiariaService {
  private appURL: string = environment.applicationUrl + 'CajaDiaria';

  constructor(private http: HttpClient) {
  }

  public index(year: string, month: string): Observable<CajaDiaria[]> {
    let params = new HttpParams();
    params = params.append('Year', year);
    params = params.append('Month', month);
    return this.http.get<CajaDiaria[]>(`${this.appURL}/Index`, {params});
  }

  public show(id: string): Observable<CajaDiaria> {
    return this.http.get<CajaDiaria>(`${this.appURL}/Show/${id}`);
  }

  public create(data: AperturaCaja): Observable<ResponseData<CajaDiaria>> {
    return this.http.post<ResponseData<CajaDiaria>>(`${this.appURL}/Create`, data);
  }

  public update(id: string, data: CerrarCaja): Observable<ResponseData<CajaDiaria>> {
    return this.http.put<ResponseData<CajaDiaria>>(`${this.appURL}/Update/${id}`, data);
  }

  public delete(id: string): Observable<ResponseData<CajaDiaria>> {
    return this.http.delete<ResponseData<CajaDiaria>>(`${this.appURL}/Delete/${id}`);
  }

}
