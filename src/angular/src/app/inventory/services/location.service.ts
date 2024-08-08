import {Injectable} from "@angular/core";
import {HttpClient, HttpParams} from "@angular/common/http";
import {environment} from "environments/environment";
import {Location, LocationItemStockDto, RespLocationDetailStock} from "../interfaces";
import {UserDataService} from "../../common/user-data.service";
import {Observable} from "rxjs";

@Injectable({
  providedIn: "root"
})
export class LocationService {
  private controller: string = "Location";
  private baseUrl: string = environment.applicationUrl + "inventory";

  constructor(
    private http: HttpClient,
    private userDataService: UserDataService) {
  }

  public index(query: string): Observable<Location[]> {
    let params = new HttpParams();
    params = params.append("query", query);
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    return this.http.get<Location[]>(url, {params});
  }

  public show(id: string): Observable<Location> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.get<Location>(url);
  }

  public detailStocks(id: string): Observable<LocationItemStockDto[]> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/Stock/${id}`;
    return this.http.get<LocationItemStockDto[]>(url);
  }

  public reponerStocks(arrId: string): Observable<RespLocationDetailStock[]> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/Reponer/${arrId}`;
    return this.http.get<RespLocationDetailStock[]>(url);
  }

  public create(data: Location): Observable<Location> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    data.companyId = companyId.trim();
    return this.http.post<Location>(url, data);
  }

  public update(id: string, data: Location): Observable<Location> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    data.companyId = companyId.trim();
    return this.http.put<Location>(url, data);
  }

  public delete(id: string): Observable<Location> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.delete<Location>(url);
  }

  public getByWarehouse(id: string): Observable<Location[]> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/Warehouse/${id}`;
    return this.http.get<Location[]>(url);
  }

}
