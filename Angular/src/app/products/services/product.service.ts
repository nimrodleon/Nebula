import {Injectable} from '@angular/core';
import {environment} from 'src/environments/environment';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs';
import {PagedResponse, ResponseData} from '../../global/interfaces';
import {Product} from '../interfaces';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private appURL: string = environment.applicationUrl + 'Product';

  constructor(
    private http: HttpClient) {
  }

  public index(pageNumber: number, pageSize: number, query: string): Observable<PagedResponse<Product>> {
    let params = new HttpParams();
    params = params.append('pageNumber', pageNumber);
    params = params.append('pageSize', pageSize);
    params = params.append('query', query);
    return this.http.get<PagedResponse<Product>>(`${this.appURL}/Index`, {params: params});
  }

  // buscador de productos para la terminal.
  public terminal(query: string): Observable<Product[]> {
    let params = new HttpParams();
    params = params.append('query', query);
    return this.http.get<Product[]>(`${this.appURL}/Terminal`, {params: params});
  }

  public show(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.appURL}/Show/${id}`);
  }

  public store(data: any): Observable<ResponseData<Product>> {
    return this.http.post<ResponseData<Product>>(`${this.appURL}/Store`, data);
  }

  public update(id: number, data: any): Observable<ResponseData<Product>> {
    return this.http.put<ResponseData<Product>>(`${this.appURL}/Update/${id}`, data);
  }

  public delete(id: number): Observable<ResponseData<Product>> {
    return this.http.delete<ResponseData<Product>>(`${this.appURL}/Destroy/${id}`);
  }

}
