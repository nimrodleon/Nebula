import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "environments/environment";
import { RepairOrder, TallerRepairOrderTicket } from "../interfaces";
import { UserDataService } from "../../common/user-data.service";
import { Observable } from "rxjs";
import { PaginationResult } from "app/common/interfaces";

@Injectable({
  providedIn: "root"
})
export class RepairOrderService {
  private controller: string = "RepairOrder";
  private baseUrl: string = environment.applicationUrl + "taller";

  constructor(
    private http: HttpClient,
    private userDataService: UserDataService) {
  }

  public index(query: string = "", page: number = 1): Observable<PaginationResult<RepairOrder>> {
    let params = new HttpParams();
    params = params.append("query", query);
    params = params.append("page", page || 1);
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    return this.http.get<PaginationResult<RepairOrder>>(url, { params });
  }

  public getMonthlyReport(year: string, month: string, query: string, page: number = 1): Observable<PaginationResult<RepairOrder>> {
    let params: HttpParams = new HttpParams();
    params = params.append("Year", year);
    params = params.append("Month", month);
    params = params.append("Query", query);
    params = params.append("page", page || 1);
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/GetMonthlyReport`;
    return this.http.get<PaginationResult<RepairOrder>>(url, { params });
  }

  public show(id: string): Observable<RepairOrder> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.get<RepairOrder>(url);
  }

  public getTicket(id: string): Observable<TallerRepairOrderTicket> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/GetTicket/${id}`;
    return this.http.get<TallerRepairOrderTicket>(url);
  }

  public create(data: RepairOrder): Observable<RepairOrder> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    return this.http.post<RepairOrder>(url, data);
  }

  public update(id: string, data: RepairOrder): Observable<RepairOrder> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.put<RepairOrder>(url, data);
  }

  public delete(id: string): Observable<RepairOrder> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.delete<RepairOrder>(url);
  }

}
