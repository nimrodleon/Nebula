import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {environment} from "environments/environment";
import {ChangeQuantityStockRequestParams, ProductStock, ProductStockInfo} from "../interfaces";
import {UserDataService} from "../../common/user-data.service";
import {Observable} from "rxjs";

@Injectable({
  providedIn: "root"
})
export class ProductStockService {
  private controller: string = "ProductStock";
  private baseUrl: string = environment.applicationUrl + "inventory";

  constructor(
    private http: HttpClient,
    private userDataService: UserDataService) {
  }

  public getStockInfos(productId: string): Observable<ProductStockInfo[]> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/GetStockInfos/${productId}`;
    return this.http.get<ProductStockInfo[]>(url);
  }

  public changeQuantity(requestParams: ChangeQuantityStockRequestParams): Observable<ProductStock> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/ChangeQuantity`;
    return this.http.post<ProductStock>(url, requestParams);
  }

  public getStockQuantity(warehouseId: string, productId: string): Observable<number> {
    const companyId: string = this.userDataService.companyId;
    const url: string = `${this.baseUrl}/${companyId}/${this.controller}/StockQuantity/${warehouseId}/${productId}`;
    return this.http.get<number>(url);
  }


}
