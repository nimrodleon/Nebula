import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable, Subject} from 'rxjs';
import {FacturadorBase, FacturadorData} from '../interfaces/facturador';
import {ConfigurationService} from '../../system/services';
import {ResponseData} from '../../global/interfaces';

@Injectable({
  providedIn: 'root'
})
export class FacturadorService {

  constructor(
    private http: HttpClient,
    private configurationService: ConfigurationService) {
  }

  public ActualizarPantalla(): Observable<FacturadorData> {
    let subject = new Subject<FacturadorData>();
    this.configurationService.show().subscribe(result => {
      this.http.get(`${result.urlApi}/api/Facturador/ActualizarPantalla`)
        .subscribe(result => subject.next(<any>result));
    });
    return subject.asObservable();
  }

  public EliminarBandeja(): Observable<FacturadorBase> {
    let subject = new Subject<FacturadorBase>();
    this.configurationService.show().subscribe(result => {
      this.http.get(`${result.urlApi}/api/Facturador/EliminarBandeja`)
        .subscribe(result => subject.next(<any>result));
    });
    return subject.asObservable();
  }

  public GenerarComprobante(invoice: number): Observable<FacturadorData> {
    let params = new HttpParams();
    params = params.append('invoice', invoice);
    let subject = new Subject<FacturadorData>();
    this.configurationService.show().subscribe(result => {
      this.http.get(`${result.urlApi}/api/Facturador/GenerarComprobante`, {params})
        .subscribe(result => subject.next(<any>result));
    });
    return subject.asObservable();
  }

  public EnviarXML(invoice: number): Observable<FacturadorData> {
    let params = new HttpParams();
    params = params.append('invoice', invoice);
    let subject = new Subject<FacturadorData>();
    this.configurationService.show().subscribe(result => {
      this.http.get(`${result.urlApi}/api/Facturador/EnviarXML`, {params})
        .subscribe(result => subject.next(<any>result));
    });
    return subject.asObservable();
  }

  // 20520485750-03-B001-00000015
  public GenerarPdf(nomArch: string): Observable<any> {
    let params = new HttpParams();
    params = params.append('nomArch', nomArch);
    let subject = new Subject<any>();
    this.configurationService.show().subscribe(result => {
      this.http.get(`${result.urlApi}/api/Facturador/GenerarPdf`, {params})
        .subscribe(result => subject.next(<any>result));
    });
    return subject.asObservable();
  }

  // 20520485750-03-B001-00000015
  public Backup(nomArch: string): Observable<ResponseData<any>> {
    let params = new HttpParams();
    params = params.append('nomArch', nomArch);
    let subject = new Subject<ResponseData<any>>();
    this.configurationService.show().subscribe(result => {
      this.http.get(`${result.urlApi}/api/Facturador/Backup`, {params})
        .subscribe(result => subject.next(<any>result));
    });
    return subject.asObservable();
  }

}
