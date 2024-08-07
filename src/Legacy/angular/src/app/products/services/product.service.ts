import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from "environments/environment";
import { Product } from "../interfaces";
import { UserDataService } from "../../common/user-data.service";
import { PaginationResult } from "app/common/interfaces";

@Injectable({
  providedIn: "root"
})
export class ProductService {
  private controller: string = "Product";
  private baseUrl: string = environment.applicationUrl + "products";

  constructor(
    private http: HttpClient,
    private userDataService: UserDataService) {
  }

  public index(query: string, page: number = 1): Observable<PaginationResult<Product>> {
    let params = new HttpParams();
    params = params.append("query", query);
    params = params.append("page", page || 1);
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    return this.http.get<PaginationResult<Product>>(url, { params });
  }

  public lista(query: string): Observable<Product[]> {
    let params = new HttpParams();
    params = params.append("query", query);
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/Lista`;
    return this.http.get<Product[]>(url, { params });
  }

  public show(id: string): Observable<Product> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.get<Product>(url);
  }

  public create(data: Product): Observable<Product> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    data.companyId = companyId.trim();
    return this.http.post<Product>(url, data);
  }

  public update(id: string, data: Product): Observable<Product> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    data.companyId = companyId.trim();
    return this.http.put<Product>(url, data);
  }

  public delete(id: string): Observable<Product> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.delete<Product>(url);
  }

  public descargarPlantilla(): Observable<Blob> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/PlantillaExcel`;
    return this.http.get(url, { responseType: "blob" });
  }

  public cargarProductos(data: FormData): Observable<any> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/CargarProductos`;
    return this.http.post(url, data);
  }


}
