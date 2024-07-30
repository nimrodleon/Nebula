import {Component, Input} from "@angular/core";
import {NgForOf} from "@angular/common";
import {ProductStockInfo} from "../../interfaces";

@Component({
  selector: "app-product-stock-modal",
  standalone: true,
  imports: [
    NgForOf
  ],
  templateUrl: "./product-stock-modal.component.html"
})
export class ProductStockModalComponent {
  @Input()
  productStockInfos: ProductStockInfo[] = new Array<ProductStockInfo>();
}
