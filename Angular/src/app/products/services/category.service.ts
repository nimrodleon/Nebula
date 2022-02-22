import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {Category} from '../interfaces';
import {ResponseData} from '../../global/interfaces';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private appURL: string = environment.applicationUrl + 'Category';

  constructor(private http: HttpClient) {
  }

  public index(query: string = ''): Observable<Category[]> {
    let params = new HttpParams();
    params = params.append('query', query);
    return this.http.get<Category[]>(`${this.appURL}/Index`, {params});
  }

  public show(id: string): Observable<Category> {
    return this.http.get<Category>(`${this.appURL}/Show/${id}`);
  }

  public create(data: Category): Observable<ResponseData<Category>> {
    return this.http.post<ResponseData<Category>>(`${this.appURL}/Create`, data);
  }

  public update(id: string, data: Category): Observable<ResponseData<Category>> {
    return this.http.put<ResponseData<Category>>(`${this.appURL}/Update/${id}`, data);
  }

  public delete(id: string): Observable<ResponseData<Category>> {
    return this.http.delete<ResponseData<Category>>(`${this.appURL}/Delete/${id}`);
  }
}
