import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable, Subject} from 'rxjs';
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
    let subject = new Subject<FacturadorData>();
    this.http.get(`${this.appURL}/ActualizarPantalla`)
      .subscribe(result => subject.next(<any>result));
    return subject.asObservable();
  }

  public EliminarBandeja(): Observable<FacturadorBase> {
    let subject = new Subject<FacturadorBase>();
    this.http.get(`${this.appURL}/EliminarBandeja`)
      .subscribe(result => subject.next(<any>result));
    return subject.asObservable();
  }

  public GenerarComprobante(invoice: number): Observable<FacturadorData> {
    let params = new HttpParams();
    params = params.append('invoice', invoice);
    let subject = new Subject<FacturadorData>();
    this.http.get(`${this.appURL}/GenerarComprobante`, {params})
      .subscribe(result => subject.next(<any>result));
    return subject.asObservable();
  }

  public EnviarXML(invoice: number): Observable<FacturadorData> {
    let params = new HttpParams();
    params = params.append('invoice', invoice);
    let subject = new Subject<FacturadorData>();
    this.http.get(`${this.appURL}/EnviarXML`, {params})
      .subscribe(result => subject.next(<any>result));
    return subject.asObservable();
  }

  public GenerarPdf(invoice: number): Observable<any> {
    let params = new HttpParams();
    params = params.append('invoice', invoice);
    let subject = new Subject<any>();
    this.http.get(`${this.appURL}/GenerarPdf`, {params})
      .subscribe(result => subject.next(<any>result));
    return subject.asObservable();
  }

  public Backup(invoice: number): Observable<ResponseData<any>> {
    let params = new HttpParams();
    params = params.append('invoice', invoice);
    let subject = new Subject<ResponseData<any>>();
    this.http.get(`${this.appURL}/Backup`, {params})
      .subscribe(result => subject.next(<any>result));
    return subject.asObservable();
  }

}
