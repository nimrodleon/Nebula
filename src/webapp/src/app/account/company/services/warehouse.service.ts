import {Injectable} from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import {environment} from "environments/environment";
import {Observable} from "rxjs";
import {Warehouse} from "../interfaces";

@Injectable({
  providedIn: "root"
})
export class WarehouseService {
  private controller: string = "Warehouse";
  private baseUrl: string = environment.applicationUrl + "account";

  constructor(
    private http: HttpClient,) {
  }

  public index(query: string = ""): Observable<Warehouse[]> {
    let params = new HttpParams();
    params = params.append("query", query);
    const url: string = `${this.baseUrl}/${this.controller}`;
    return this.http.get<Warehouse[]>(url, {params});
  }

  public show(id: string): Observable<Warehouse> {
    const url: string = `${this.baseUrl}/${this.controller}/${id}`;
    return this.http.get<Warehouse>(url);
  }

  public create(data: Warehouse): Observable<Warehouse> {
    const url: string = `${this.baseUrl}/${this.controller}`;
    return this.http.post<Warehouse>(url, data);
  }

  public update(id: string, data: Warehouse): Observable<Warehouse> {
    const url: string = `${this.baseUrl}/${this.controller}/${id}`;
    return this.http.put<Warehouse>(url, data);
  }

  public delete(id: string): Observable<Warehouse> {
    const url: string = `${this.baseUrl}/${this.controller}/${id}`;
    return this.http.delete<Warehouse>(url);
  }

}
