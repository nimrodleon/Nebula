import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {AperturaCaja, CajaDiaria, CerrarCaja} from '../interfaces';
import {ResponseData} from '../../global/interfaces';

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
    return this.http.get<CajaDiaria[]>(`${this.appURL}/Index`, {params: params});
  }

  public show(id: number): Observable<CajaDiaria> {
    return this.http.get<CajaDiaria>(`${this.appURL}/Show/${id}`);
  }

  public store(data: AperturaCaja): Observable<ResponseData<CajaDiaria>> {
    return this.http.post<ResponseData<CajaDiaria>>(`${this.appURL}/Store`, data);
  }

  public update(id: number, data: CerrarCaja): Observable<ResponseData<CajaDiaria>> {
    return this.http.put<ResponseData<CajaDiaria>>(`${this.appURL}/Update/${id}`, data);
  }

}
