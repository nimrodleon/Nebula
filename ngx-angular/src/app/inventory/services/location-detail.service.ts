import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {environment} from "environments/environment";
import {UserDataService} from "../../common/user-data.service";
import {LocationDetail} from "../interfaces";
import {Observable} from "rxjs";

@Injectable({
  providedIn: "root"
})
export class LocationDetailService {
  private controller: string = "LocationDetail";
  private baseUrl: string = environment.applicationUrl + "inventory";

  constructor(
    private http: HttpClient,
    private userDataService: UserDataService) {
  }

  public index(id: string): Observable<LocationDetail[]> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.get<LocationDetail[]>(url);
  }

  public show(id: string): Observable<LocationDetail> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/Show/${id}`;
    return this.http.get<LocationDetail>(url);
  }

  public create(data: LocationDetail): Observable<LocationDetail> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    return this.http.post<LocationDetail>(url, data);
  }

  public update(id: string, data: LocationDetail): Observable<LocationDetail> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.put<LocationDetail>(url, data);
  }

  public delete(id: string): Observable<LocationDetail> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.delete<LocationDetail>(url);
  }

  public countDocuments(id: string): Observable<number> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/CountDocuments/${id}`;
    return this.http.get<number>(url);
  }
}
