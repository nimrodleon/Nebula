import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {PeopleDocType} from '../interfaces';
import {ResponseData} from '../../global/interfaces';

@Injectable({
  providedIn: 'root'
})
export class PeopleDocTypeService {
  private appURL: string = environment.applicationUrl + 'PeopleDocType';

  constructor(private http: HttpClient) {
  }

  public index(query: string = ''): Observable<PeopleDocType[]> {
    let params = new HttpParams();
    params = params.append('query', query);
    return this.http.get<PeopleDocType[]>(`${this.appURL}/Index`, {params: params});
  }

  public show(id: string): Observable<PeopleDocType> {
    return this.http.get<PeopleDocType>(`${this.appURL}/Show/${id}`);
  }

  public create(data: PeopleDocType): Observable<ResponseData<PeopleDocType>> {
    return this.http.post<ResponseData<PeopleDocType>>(`${this.appURL}/Create`, data);
  }

  public update(id: string, data: PeopleDocType): Observable<ResponseData<PeopleDocType>> {
    return this.http.put<ResponseData<PeopleDocType>>(`${this.appURL}/Update/${id}`, data);
  }

  public delete(id: string): Observable<ResponseData<PeopleDocType>> {
    return this.http.delete<ResponseData<PeopleDocType>>(`${this.appURL}/Delete/${id}`);
  }

}
