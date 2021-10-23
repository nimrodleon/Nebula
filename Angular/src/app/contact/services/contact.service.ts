import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {Contact} from '../interfaces';
import {PagedResponse, ResponseData} from '../../global/interfaces';

@Injectable({
  providedIn: 'root'
})
export class ContactService {
  private appURL: string = environment.applicationUrl + 'Contact';

  constructor(private http: HttpClient) {
  }

  public index(pageNumber: number, pageSize: number, query: string): Observable<PagedResponse<Contact[]>> {
    let params = new HttpParams();
    params = params.append('pageNumber', pageNumber);
    params = params.append('pageSize', pageSize);
    params = params.append('query', query);
    return this.http.get<PagedResponse<Contact[]>>(`${this.appURL}/Index`, {params: params});
  }

  public show(id: number): Observable<Contact> {
    return this.http.get<Contact>(`${this.appURL}/Show/${id}`);
  }

  public store(data: Contact): Observable<ResponseData<Contact>> {
    return this.http.post<ResponseData<Contact>>(`${this.appURL}/Store`, data);
  }

  public update(id: number, data: Contact): Observable<ResponseData<Contact>> {
    return this.http.put<ResponseData<Contact>>(`${this.appURL}/Update/${id}`, data);
  }

  public destroy(id: number): Observable<ResponseData<Contact>> {
    return this.http.delete<ResponseData<Contact>>(`${this.appURL}/Destroy/${id}`);
  }

}
