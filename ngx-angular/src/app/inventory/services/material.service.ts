import {Injectable} from "@angular/core";
import {environment} from "environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {UserDataService} from "../../common/user-data.service";
import {Material, MaterialDto} from "../interfaces";
import {Observable} from "rxjs";

@Injectable({
  providedIn: "root"
})
export class MaterialService {
  private controller: string = "Material";
  private baseUrl: string = environment.applicationUrl + "inventory";

  constructor(
    private http: HttpClient,
    private userDataService: UserDataService) {
  }

  public index(year: string, month: string): Observable<Material[]> {
    let params = new HttpParams();
    params = params.append("Year", year);
    params = params.append("Month", month);
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    return this.http.get<Material[]>(url, {params});
  }

  public getByContactId(year: string, month: string, contactId: string): Observable<Material[]> {
    let params = new HttpParams();
    params = params.append("Year", year);
    params = params.append("Month", month);
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/Contact/${contactId}`;
    return this.http.get<Material[]>(url, {params});
  }

  public show(id: string): Observable<Material> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.get<Material>(url);
  }

  public create(data: Material): Observable<Material> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    data.companyId = companyId.trim();
    return this.http.post<Material>(url, data);
  }

  public update(id: string, data: Material): Observable<Material> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    data.companyId = companyId.trim();
    return this.http.put<Material>(url, data);
  }

  public delete(id: string): Observable<Material> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.delete<Material>(url);
  }

  public validate(id: string): Observable<MaterialDto> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/Validate/${id}`;
    return this.http.get<MaterialDto>(url);
  }

}
