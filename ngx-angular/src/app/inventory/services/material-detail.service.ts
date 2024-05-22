import {Injectable} from "@angular/core";
import {environment} from "environments/environment";
import {HttpClient} from "@angular/common/http";
import {UserDataService} from "../../common/user-data.service";
import {MaterialDetail} from "../interfaces";
import {Observable} from "rxjs";

@Injectable({
  providedIn: "root"
})
export class MaterialDetailService {
  private controller: string = "MaterialDetail";
  private baseUrl: string = environment.applicationUrl + "inventory";

  constructor(
    private http: HttpClient,
    private userDataService: UserDataService) {
  }

  public index(id: string): Observable<MaterialDetail[]> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.get<MaterialDetail[]>(url);
  }

  public show(id: string): Observable<MaterialDetail> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/Show/${id}`;
    return this.http.get<MaterialDetail>(url);
  }

  public create(data: MaterialDetail): Observable<MaterialDetail> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    data.companyId = companyId.trim();
    return this.http.post<MaterialDetail>(url, data);
  }

  public update(id: string, data: MaterialDetail): Observable<MaterialDetail> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    data.companyId = companyId.trim();
    return this.http.put<MaterialDetail>(url, data);
  }

  public delete(id: string): Observable<MaterialDetail> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.delete<MaterialDetail>(url);
  }

  public countDocuments(id: string): Observable<number> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/CountDocuments/${id}`;
    return this.http.get<number>(url);
  }

}
