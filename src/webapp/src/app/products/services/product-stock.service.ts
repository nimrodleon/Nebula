import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {environment} from "environments/environment";
import {Observable} from "rxjs";
import {ChangeQuantityStockRequestParams, ProductStock, ProductStockInfo} from "../interfaces";

@Injectable({
  providedIn: "root"
})
export class ProductStockService {
  private controller: string = "ProductStock";
  private baseUrl: string = environment.applicationUrl + "inventory";

  constructor(
    private http: HttpClient,) {
  }

  public getStockInfos(productId: string): Observable<ProductStockInfo[]> {
    const url: string = `${this.baseUrl}/${this.controller}/GetStockInfos/${productId}`;
    return this.http.get<ProductStockInfo[]>(url);
  }

  public changeQuantity(requestParams: ChangeQuantityStockRequestParams): Observable<ProductStock> {
    const url: string = `${this.baseUrl}/${this.controller}/ChangeQuantity`;
    return this.http.post<ProductStock>(url, requestParams);
  }

  public getStockQuantity(warehouseId: string, productId: string): Observable<number> {
    const url: string = `${this.baseUrl}/${this.controller}/StockQuantity/${warehouseId}/${productId}`;
    return this.http.get<number>(url);
  }
}
