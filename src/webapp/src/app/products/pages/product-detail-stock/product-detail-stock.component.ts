import {Component, OnInit} from "@angular/core";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {faEdit} from "@fortawesome/free-solid-svg-icons";
import {ProductDetailService} from "../../services";
import {ActivatedRoute} from "@angular/router";
import {
  ProductDetailContainerComponent
} from "../../components/product-detail-container/product-detail-container.component";
import {NgClass, NgForOf} from "@angular/common";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {toastError, toastSuccess} from "../../../common/interfaces";
import {ChangeQuantityStockRequestParams, ProductStockInfo} from "../../interfaces";

declare const bootstrap: any;

@Component({
  selector: "app-product-detail-stock",
  standalone: true,
  imports: [
    ProductDetailContainerComponent,
    NgForOf,
    FaIconComponent,
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: "./product-detail-stock.component.html"
})
export class ProductDetailStockComponent implements OnInit {
  faEdit = faEdit;
  changeQtyModal: any;
  quantityForm: FormGroup = this.fb.group({
    quantity: [null, [Validators.required]]
  });
  currentProductStockInfo: ProductStockInfo = new ProductStockInfo();

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private productDetailService: ProductDetailService) {
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      const id: string = params.get("id") || "";
      this.productDetailService.cargarStockDeProductos(id);
    });
    this.changeQtyModal = new bootstrap.Modal("#changeQtyModal");
  }

  public get producto() {
    return this.productDetailService.producto;
  }

  public get productStockInfos() {
    return this.productDetailService.productStockInfos;
  }

  // Validar formulario.
  public inputIsInvalid(field: string) {
    return this.quantityForm.controls[field].errors
      && this.quantityForm.controls[field].touched;
  }

  // Abrir modal cambiar cantidad.
  public showChangeQtyModal(item: ProductStockInfo): void {
    this.currentProductStockInfo = item;
    this.quantityForm.reset({quantity: item.quantity});
    this.changeQtyModal.show();
  }

  // Guardar cambios de cantidad.
  public saveChange(): void {
    if (this.quantityForm.invalid) {
      this.quantityForm.markAllAsTouched();
      toastError("Ingrese la información en el campo requeridos!");
      return;
    }
    const changeQuantityStock = new ChangeQuantityStockRequestParams();
    changeQuantityStock.warehouseId = this.currentProductStockInfo.warehouseId;
    changeQuantityStock.productId = this.currentProductStockInfo.productId;
    changeQuantityStock.quantity = this.quantityForm.get("quantity")?.value;
    this.productDetailService.changeQuantityStock(changeQuantityStock)
      .subscribe(result => {
        if (result) {
          toastSuccess("La cantidad del producto ha sido actualizado!");
          this.changeQtyModal.hide();
        }
      });
  }
}
