import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {environment} from "environments/environment";
import {UserDataService} from "../../common/user-data.service";
import {Observable} from "rxjs";
import {ItemRepairOrder} from "../interfaces";

@Injectable({
  providedIn: "root"
})
export class ItemRepairOrderService {
  private controller: string = "ItemRepairOrder";
  private baseUrl: string = environment.applicationUrl + "taller";

  constructor(
    private http: HttpClient,
    private userDataService: UserDataService) {
  }

  public index(id: string): Observable<ItemRepairOrder[]> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.get<ItemRepairOrder[]>(url);
  }

  public create(data: ItemRepairOrder): Observable<ItemRepairOrder> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}`;
    return this.http.post<ItemRepairOrder>(url, data);
  }

  public update(id: string, data: ItemRepairOrder): Observable<ItemRepairOrder> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.put<ItemRepairOrder>(url, data);
  }

  public delete(id: string): Observable<ItemRepairOrder> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/${id}`;
    return this.http.delete<ItemRepairOrder>(url);
  }

}
