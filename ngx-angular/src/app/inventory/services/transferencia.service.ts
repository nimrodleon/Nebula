import {Injectable} from "@angular/core";
import {environment} from "environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {Transferencia, TransferenciaDto} from "../interfaces";
import {Observable} from "rxjs";
import {UserDataService} from "../../common/user-data.service";

@Injectable({
  providedIn: "root"
})
export class TransferenciaService {
  private controller: string = "Transferencia";
  private baseUrl: string = environment.applicationUrl + "inventory";

  constructor(
    private http: HttpClient,
    private userDataService: UserDataService) {
  }

  public index(year: string, month: string): Observable<Transferencia[]> {
    let params = new HttpParams();
    params = params.append("Year", year);
    params = params.append("Month", month);
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    return this.http.get<Transferencia[]>(url, {params});
  }

  public show(id: string): Observable<Transferencia> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.get<Transferencia>(url);
  }

  public create(data: Transferencia): Observable<Transferencia> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    data.companyId = companyId.trim();
    return this.http.post<Transferencia>(url, data);
  }

  public update(id: string, data: Transferencia): Observable<Transferencia> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    data.companyId = companyId.trim();
    return this.http.put<Transferencia>(url, data);
  }

  public delete(id: string): Observable<Transferencia> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.delete<Transferencia>(url);
  }

  public validate(id: string): Observable<TransferenciaDto> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/Validate/${id}`;
    return this.http.get<TransferenciaDto>(url);
  }

}
