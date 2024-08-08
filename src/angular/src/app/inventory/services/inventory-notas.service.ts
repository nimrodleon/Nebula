import {Injectable} from "@angular/core";
import {environment} from "environments/environment";
import {HttpClient, HttpParams} from "@angular/common/http";
import {InventoryNotas, InventoryNoteDto} from "../interfaces";
import {Observable} from "rxjs";
import {UserDataService} from "../../common/user-data.service";

@Injectable({
  providedIn: "root"
})
export class InventoryNotasService {
  private controller: string = "InventoryNotas";
  private baseUrl: string = environment.applicationUrl + "inventory";

  constructor(
    private http: HttpClient,
    private userDataService: UserDataService) {
  }

  public index(year: string, month: string): Observable<InventoryNotas[]> {
    let params = new HttpParams();
    params = params.append("Year", year);
    params = params.append("Month", month);
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    return this.http.get<InventoryNotas[]>(url, {params});
  }

  public show(id: string): Observable<InventoryNotas> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.get<InventoryNotas>(url);
  }

  public create(data: InventoryNotas): Observable<InventoryNotas> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    data.companyId = companyId.trim();
    return this.http.post<InventoryNotas>(url, data);
  }

  public update(id: string, data: InventoryNotas): Observable<InventoryNotas> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    data.companyId = companyId.trim();
    return this.http.put<InventoryNotas>(url, data);
  }

  public delete(id: string): Observable<InventoryNotas> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.delete<InventoryNotas>(url);
  }

  public validate(id: string): Observable<InventoryNoteDto> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/Validate/${id}`;
    return this.http.get<InventoryNoteDto>(url);
  }
}
