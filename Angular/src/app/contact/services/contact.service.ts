import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {ResponseData} from 'src/app/global/interfaces';
import {Contact} from '../interfaces';

@Injectable({
  providedIn: 'root'
})
export class ContactService {
  private appURL: string = environment.applicationUrl + 'Contact';

  constructor(private http: HttpClient) {
  }

  public index(query: string): Observable<Array<Contact>> {
    let params = new HttpParams();
    params = params.append('query', query);
    return this.http.get<Array<Contact>>(`${this.appURL}/Index`, {params});
  }

  public show(id: string): Observable<Contact> {
    return this.http.get<Contact>(`${this.appURL}/Show/${id}`);
  }

  public create(data: Contact): Observable<ResponseData<Contact>> {
    return this.http.post<ResponseData<Contact>>(`${this.appURL}/Create`, data);
  }

  public update(id: string, data: Contact): Observable<ResponseData<Contact>> {
    return this.http.put<ResponseData<Contact>>(`${this.appURL}/Update/${id}`, data);
  }

  public delete(id: string): Observable<ResponseData<Contact>> {
    return this.http.delete<ResponseData<Contact>>(`${this.appURL}/Delete/${id}`);
  }

}
