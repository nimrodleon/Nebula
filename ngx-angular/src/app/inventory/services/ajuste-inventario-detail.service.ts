import {Injectable} from "@angular/core";
import {environment} from "environments/environment";
import {HttpClient} from "@angular/common/http";
import {UserDataService} from "../../common/user-data.service";
import {AjusteInventarioDetail} from "../interfaces";
import {Observable} from "rxjs";

@Injectable({
  providedIn: "root"
})
export class AjusteInventarioDetailService {
  private controller: string = "AjusteInventarioDetail";
  private baseUrl: string = environment.applicationUrl + "inventory";

  constructor(
    private http: HttpClient,
    private userDataService: UserDataService) {
  }

  public index(id: string): Observable<Array<AjusteInventarioDetail>> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.get<Array<AjusteInventarioDetail>>(url);
  }

  public show(id: string): Observable<AjusteInventarioDetail> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/Show/${id}`;
    return this.http.get<AjusteInventarioDetail>(url);
  }

  public create(data: AjusteInventarioDetail): Observable<AjusteInventarioDetail> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    data.companyId = companyId.trim();
    return this.http.post<AjusteInventarioDetail>(url, data);
  }

  public update(id: string, data: AjusteInventarioDetail): Observable<AjusteInventarioDetail> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    data.companyId = companyId.trim();
    return this.http.put<AjusteInventarioDetail>(url, data);
  }

  public delete(id: string): Observable<AjusteInventarioDetail> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.delete<AjusteInventarioDetail>(url);
  }

  public countDocuments(id: string): Observable<number> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/CountDocuments/${id}`;
    return this.http.get<number>(url);
  }
}
