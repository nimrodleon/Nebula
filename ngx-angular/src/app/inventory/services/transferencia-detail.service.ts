import {Injectable} from "@angular/core";
import {environment} from "environments/environment";
import {HttpClient} from "@angular/common/http";
import {UserDataService} from "../../common/user-data.service";
import {TransferenciaDetail} from "../interfaces";
import {Observable} from "rxjs";

@Injectable({
  providedIn: "root"
})
export class TransferenciaDetailService {
  private controller: string = "TransferenciaDetail";
  private baseUrl: string = environment.applicationUrl + "inventory";

  constructor(
    private http: HttpClient,
    private userDataService: UserDataService) {
  }

  public index(id: string): Observable<TransferenciaDetail[]> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.get<TransferenciaDetail[]>(url);
  }

  public show(id: string): Observable<TransferenciaDetail> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/Show/${id}`;
    return this.http.get<TransferenciaDetail>(url);
  }

  public create(data: TransferenciaDetail): Observable<TransferenciaDetail> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    return this.http.post<TransferenciaDetail>(url, data);
  }

  public update(id: string, data: TransferenciaDetail): Observable<TransferenciaDetail> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.put<TransferenciaDetail>(url, data);
  }

  public delete(id: string): Observable<TransferenciaDetail> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.delete<TransferenciaDetail>(url);
  }

  public countDocuments(id: string): Observable<number> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/CountDocuments/${id}`;
    return this.http.get<number>(url);
  }
}
