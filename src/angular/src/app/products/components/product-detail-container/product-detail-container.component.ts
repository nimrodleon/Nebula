import {Component, OnInit} from "@angular/core";
import {ActivatedRoute, RouterLink, RouterLinkActive} from "@angular/router";
import {ProductDetailService} from "../../services";
import {Product} from "../../interfaces";
import {ProductContainerComponent} from "app/common/containers/product-container/product-container.component";

@Component({
  selector: "app-product-detail-container",
  standalone: true,
  imports: [
    RouterLink,
    ProductContainerComponent,
    RouterLinkActive
  ],
  templateUrl: "./product-detail-container.component.html"
})
export class ProductDetailContainerComponent implements OnInit {
  companyId: string = "";
  protected readonly Product = Product;

  constructor(
    private activatedRoute: ActivatedRoute,
    private productDetailService: ProductDetailService) {
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
      const id: string = params.get("id") || "";
      this.productDetailService.cargarProducto(id);
    });
  }

  public get producto(): Product {
    return this.productDetailService.producto;
  }

}
