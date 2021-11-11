import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from 'src/environments/environment';
import {Observable} from 'rxjs';
import {TypeOperationSunat} from '../interfaces';

@Injectable({
  providedIn: 'root'
})
export class SunatService {
  private appURL: string = environment.applicationUrl + 'Sunat';

  constructor(
    private http: HttpClient) {
  }

  public typeOperation(): Observable<TypeOperationSunat[]> {
    return this.http.get<TypeOperationSunat[]>(`${this.appURL}/TypeOperation`);
  }

}
