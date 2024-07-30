import {Injectable} from "@angular/core";
import {ChangeQuantityStockRequestParams, Product, ProductStockInfo} from "../interfaces";
import {ProductService} from "./product.service";
import {map, Observable} from "rxjs";
import {ProductStockService} from "./product-stock.service";
import _ from "lodash";

@Injectable({
  providedIn: "root"
})
export class ProductDetailService {
  private _producto: Product = new Product();
  private _productStockInfos = new Array<ProductStockInfo>();

  constructor(
    private productService: ProductService,
     private productStockService: ProductStockService ) {
  }

  public cargarProducto(productId: string): void {
    this.productService.show(productId)
      .subscribe(result => this._producto = result);
  }

  public get producto(): Product {
    return this._producto;
  }

  public set producto(value: Product) {
    this._producto = value;
  }

  public cargarStockDeProductos(productId: string): void {
     this.productStockService.getStockInfos(productId)
       .subscribe(result => this._productStockInfos = result);
  }

  public get productStockInfos(): Array<ProductStockInfo> {
    return this._productStockInfos;
  }

  public changeQuantityStock(requestParams: ChangeQuantityStockRequestParams): Observable<boolean> {
    return this.productStockService.changeQuantity(requestParams)
      .pipe(map(result => {
        this._productStockInfos = _.map(this._productStockInfos, item => {
          if (item.warehouseId === result.warehouseId && item.productId === result.productId)
            item.quantity = result.quantity;
          return item;
        });
        return true;
      }));
  }

}
