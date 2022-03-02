import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {ResponseData} from '../../global/interfaces';
import {Product} from '../interfaces';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private appURL: string = environment.applicationUrl + 'Product';

  constructor(
    private http: HttpClient) {
  }

  public index(query: string): Observable<Array<Product>> {
    let params = new HttpParams();
    params = params.append('query', query);
    return this.http.get<Array<Product>>(`${this.appURL}/Index`, {params});
  }

  public show(id: string): Observable<Product> {
    return this.http.get<Product>(`${this.appURL}/Show/${id}`);
  }

  public create(data: Product): Observable<ResponseData<Product>> {
    return this.http.post<ResponseData<Product>>(`${this.appURL}/Create`, data);
  }

  public update(id: string, data: Product): Observable<ResponseData<Product>> {
    return this.http.put<ResponseData<Product>>(`${this.appURL}/Update/${id}`, data);
  }

  public delete(id: number): Observable<ResponseData<Product>> {
    return this.http.delete<ResponseData<Product>>(`${this.appURL}/Delete/${id}`);
  }
}
