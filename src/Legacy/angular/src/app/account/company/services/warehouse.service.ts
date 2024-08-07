import {Injectable} from "@angular/core";
import {HttpClient, HttpParams} from "@angular/common/http";
import {environment} from "environments/environment";
import {UserDataService} from "app/common/user-data.service";
import {Observable} from "rxjs";
import {Warehouse} from "../interfaces";

@Injectable({
  providedIn: "root"
})
export class WarehouseService {
  private controller: string = "Warehouse";
  private baseUrl: string = environment.applicationUrl + "account";

  constructor(
    private http: HttpClient,
    private userDataService: UserDataService) {
  }

  public index(query: string = "", companyId: string = ""): Observable<Warehouse[]> {
    let params = new HttpParams();
    params = params.append("query", query);
    if (companyId === "") companyId = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    return this.http.get<Warehouse[]>(url, {params});
  }

  public show(id: string, companyId: string = ""): Observable<Warehouse> {
    if (companyId === "") companyId = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.get<Warehouse>(url);
  }

  public create(data: Warehouse, companyId: string = ""): Observable<Warehouse> {
    data.companyId = companyId.trim();
    if (companyId === "") companyId = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    return this.http.post<Warehouse>(url, data);
  }

  public update(id: string, data: Warehouse, companyId: string = ""): Observable<Warehouse> {
    data.companyId = companyId.trim();
    if (companyId === "") companyId = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.put<Warehouse>(url, data);
  }

  public delete(id: string, companyId: string = ""): Observable<Warehouse> {
    if (companyId === "") companyId = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.delete<Warehouse>(url);
  }

}
