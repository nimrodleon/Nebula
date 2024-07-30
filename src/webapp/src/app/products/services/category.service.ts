import {Injectable} from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import {Observable} from "rxjs";
import {environment} from "environments/environment";
import {Category} from "../interfaces";

@Injectable({
  providedIn: "root"
})
export class CategoryService {
  private controller: string = "Category";
  private baseUrl: string = environment.applicationUrl + "products";

  constructor(
    private http: HttpClient) {
  }

  public index(query: string = ""): Observable<Category[]> {
    let params = new HttpParams();
    params = params.append("query", query);
    const url: string = `${this.baseUrl}/${this.controller}`;
    return this.http.get<Category[]>(url, {params});
  }

  public show(id: string): Observable<Category> {
    const url: string = `${this.baseUrl}/${this.controller}/${id}`;
    return this.http.get<Category>(url);
  }

  public create(data: Category): Observable<Category> {
    const url: string = `${this.baseUrl}/${this.controller}`;
    return this.http.post<Category>(url, data);
  }

  public update(id: string, data: Category): Observable<Category> {
    const url: string = `${this.baseUrl}/${this.controller}/${id}`;
    return this.http.put<Category>(url, data);
  }

  public delete(id: string): Observable<Category> {
    const url: string = `${this.baseUrl}/${this.controller}/${id}`;
    return this.http.delete<Category>(url);
  }

}
