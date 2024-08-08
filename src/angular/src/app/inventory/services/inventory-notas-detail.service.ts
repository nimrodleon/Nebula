import {Injectable} from "@angular/core";
import {environment} from "environments/environment";
import {HttpClient} from "@angular/common/http";
import {UserDataService} from "../../common/user-data.service";
import {InventoryNotasDetail} from "../interfaces";
import {Observable} from "rxjs";

@Injectable({
  providedIn: "root"
})
export class InventoryNotasDetailService {
  private controller: string = "InventoryNotasDetail";
  private baseUrl: string = environment.applicationUrl + "inventory";

  constructor(
    private http: HttpClient,
    private userDataService: UserDataService) {
  }

  public index(id: string): Observable<Array<InventoryNotasDetail>> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.get<Array<InventoryNotasDetail>>(url);
  }

  public show(id: string): Observable<InventoryNotasDetail> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/Show/${id}`;
    return this.http.get<InventoryNotasDetail>(url);
  }

  public create(data: InventoryNotasDetail): Observable<InventoryNotasDetail> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    data.companyId = companyId.trim();
    return this.http.post<InventoryNotasDetail>(url, data);
  }

  public update(id: string, data: InventoryNotasDetail): Observable<InventoryNotasDetail> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    data.companyId = companyId.trim();
    return this.http.put<InventoryNotasDetail>(url, data);
  }

  public delete(id: string): Observable<InventoryNotasDetail> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.delete<InventoryNotasDetail>(url);
  }

  public countDocuments(id: string): Observable<number> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/CountDocuments/${id}`;
    return this.http.get<number>(url);
  }
}
