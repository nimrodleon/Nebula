import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {PeopleDocType} from '../interfaces';

@Injectable({
  providedIn: 'root'
})
export class PeopleDocTypeService {
  private appURL: string = environment.applicationUrl + 'PeopleDocType';

  constructor(private http: HttpClient) {
  }

  public index(): Observable<PeopleDocType[]> {
    return this.http.get<PeopleDocType[]>(`${this.appURL}/Index`);
  }

  public show(id: string): Observable<PeopleDocType> {
    return this.http.get<PeopleDocType>(`${this.appURL}/Show/${id}`);
  }

  public store(data: PeopleDocType): Observable<any> {
    return this.http.post(`${this.appURL}/Store`, data);
  }

  public update(id: string, data: PeopleDocType): Observable<any> {
    return this.http.put(`${this.appURL}/Update/${id}`, data);
  }

  public destroy(id: string): Observable<any> {
    return this.http.delete(`${this.appURL}/Destroy/${id}`);
  }

}
