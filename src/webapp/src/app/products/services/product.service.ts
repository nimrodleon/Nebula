import {Injectable} from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import {Observable} from "rxjs";
import {environment} from "environments/environment";
import {Product} from "../interfaces";
import {PaginationResult} from "app/common/interfaces";

@Injectable({
  providedIn: "root"
})
export class ProductService {
  private controller: string = "Product";
  private baseUrl: string = environment.applicationUrl + "products";

  constructor(
    private http: HttpClient,) {
  }

  public index(query: string, page: number = 1): Observable<PaginationResult<Product>> {
    let params = new HttpParams();
    params = params.append("query", query);
    params = params.append("page", page || 1);
    const url: string = `${this.baseUrl}/${this.controller}`;
    return this.http.get<PaginationResult<Product>>(url, {params});
  }

  public lista(query: string): Observable<Product[]> {
    let params = new HttpParams();
    params = params.append("query", query);
    const url: string = `${this.baseUrl}/${this.controller}/Lista`;
    return this.http.get<Product[]>(url, {params});
  }

  public show(id: string): Observable<Product> {
    const url: string = `${this.baseUrl}/${this.controller}/${id}`;
    return this.http.get<Product>(url);
  }

  public create(data: Product): Observable<Product> {
    const url: string = `${this.baseUrl}/${this.controller}`;
    return this.http.post<Product>(url, data);
  }

  public update(id: string, data: Product): Observable<Product> {
    const url: string = `${this.baseUrl}/${this.controller}/${id}`;
    return this.http.put<Product>(url, data);
  }

  public delete(id: string): Observable<Product> {
    const url: string = `${this.baseUrl}/${this.controller}/${id}`;
    return this.http.delete<Product>(url);
  }

  public descargarPlantilla(): Observable<Blob> {
    const url: string = `${this.baseUrl}/${this.controller}/PlantillaExcel`;
    return this.http.get(url, {responseType: "blob"});
  }

  public cargarProductos(data: FormData): Observable<any> {
    const url: string = `${this.baseUrl}/${this.controller}/CargarProductos`;
    return this.http.post(url, data);
  }

}
