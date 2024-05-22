import {Injectable} from "@angular/core";
import {environment} from "environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {UserDataService} from "../../common/user-data.service";
import {AjusteInventario, AjusteInventarioDto} from "../interfaces";
import {Observable} from "rxjs";

@Injectable({
  providedIn: "root"
})
export class AjusteInventarioService {
  private controller: string = "AjusteInventario";
  private baseUrl: string = environment.applicationUrl + "inventory";

  constructor(
    private http: HttpClient,
    private userDataService: UserDataService) {
  }

  public index(year: string, month: string): Observable<AjusteInventario[]> {
    let params = new HttpParams();
    params = params.append("Year", year);
    params = params.append("Month", month);
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    return this.http.get<AjusteInventario[]>(url, {params});
  }

  public show(id: string): Observable<AjusteInventario> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.get<AjusteInventario>(url);
  }

  public create(data: AjusteInventario): Observable<AjusteInventario> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    data.companyId = companyId.trim();
    return this.http.post<AjusteInventario>(url, data);
  }

  public update(id: string, data: AjusteInventario): Observable<AjusteInventario> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    data.companyId = companyId.trim();
    return this.http.put<AjusteInventario>(url, data);
  }

  public delete(id: string): Observable<AjusteInventario> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.delete<AjusteInventario>(url);
  }

  public validate(id: string): Observable<AjusteInventarioDto> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/Validate/${id}`;
    return this.http.get<AjusteInventarioDto>(url);
  }

}
