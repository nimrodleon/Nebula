import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {Configuration} from '../interfaces';
import {ResponseData} from '../../global/interfaces';

@Injectable({
  providedIn: 'root'
})
export class ConfigurationService {
  private appURL: string = environment.applicationUrl + 'Configuration';

  constructor(
    private http: HttpClient) {
  }

  public show(): Observable<Configuration> {
    return this.http.get<Configuration>(`${this.appURL}/Show`);
  }

  public update(id: number, data: Configuration): Observable<ResponseData<Configuration>> {
    return this.http.put<ResponseData<Configuration>>(`${this.appURL}/Update/${id}`, data);
  }
}
