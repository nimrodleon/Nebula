import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {FacturadorBase, FacturadorData} from '../interfaces/facturador';
import {ResponseData} from '../../global/interfaces';

@Injectable({
  providedIn: 'root'
})
export class FacturadorService {
  private appURL: string = environment.applicationUrl + 'Facturador';

  constructor(private http: HttpClient) {
  }

  public ActualizarPantalla(): Observable<FacturadorData> {
    return this.http.get<FacturadorData>(`${this.appURL}/ActualizarPantalla`);
  }

  public EliminarBandeja(): Observable<FacturadorBase> {
    return this.http.get<FacturadorData>(`${this.appURL}/EliminarBandeja`);
  }

  public GenerarComprobante(invoice: number): Observable<FacturadorData> {
    let params = new HttpParams();
    params = params.append('invoice', invoice);
    return this.http.get<FacturadorData>(`${this.appURL}/GenerarComprobante`, {params});
  }

  public EnviarXML(invoice: number): Observable<FacturadorData> {
    let params = new HttpParams();
    params = params.append('invoice', invoice);
    return this.http.get<FacturadorData>(`${this.appURL}/EnviarXML`, {params});
  }

  public GenerarPdf(invoice: number): Observable<any> {
    let params = new HttpParams();
    params = params.append('invoice', invoice);
    return this.http.get(`${this.appURL}/GenerarPdf`, {params});
  }

  public Backup(invoice: number): Observable<ResponseData<any>> {
    let params = new HttpParams();
    params = params.append('invoice', invoice);
    return this.http.get<ResponseData<any>>(`${this.appURL}/Backup`, {params});
  }
}
