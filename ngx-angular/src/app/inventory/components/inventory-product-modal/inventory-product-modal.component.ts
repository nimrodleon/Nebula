import {Component, EventEmitter, Input, OnInit, Output} from "@angular/core";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {faBox, faTags} from "@fortawesome/free-solid-svg-icons";
import {select2Productos, toastError} from "app/common/interfaces";
import {InventoryProductDataModal} from "../../interfaces";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {NgClass} from "@angular/common";

declare const jQuery: any;

@Component({
  selector: "app-inventory-product-modal",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FaIconComponent,
    NgClass,
  ],
  templateUrl: "./inventory-product-modal.component.html"
})
export class InventoryProductModalComponent implements OnInit {
  faBox = faBox;
  faTags = faTags;
  @Input()
  dataModal = new InventoryProductDataModal();
  productForm: FormGroup = this.fb.group({
    productId: [null, [Validators.required]],
    cantidad: [null, [Validators.required]],
  });
  @Output()
  responseData = new EventEmitter<InventoryProductDataModal>();

  constructor(private fb: FormBuilder) {
  }

  ngOnInit(): void {
    const producto = select2Productos("#producto",
      jQuery("#inventory-product-modal"))
      .on("select2:select", (e: any) => {
        const data = e.params.data;
        this.productForm.controls["productId"].setValue(data.id);
        this.dataModal.inventoryProduct.productName = data.description;
      });
    const myModal: any = document.querySelector("#inventory-product-modal");
    myModal.addEventListener("shown.bs.modal", () => {
      if (this.dataModal.type === "EDIT") {
        const {productId, productName, cantidad} = this.dataModal.inventoryProduct;
        this.productForm.reset({
          productId: productId, cantidad: cantidad
        });
        const newOption = new Option(productName, productId, true, true);
        producto.append(newOption).trigger("change");
      }
    });
    myModal.addEventListener("hidden.bs.modal", () => {
      this.productForm.reset({productId: null, cantidad: null});
      producto.val(null).trigger("change");
    });
  }

  public inputIsInvalid(field: string) {
    return this.productForm.controls[field].errors
      && this.productForm.controls[field].touched;
  }

  public saveChange(): void {
    const {productId, cantidad} = this.productForm.value;
    this.dataModal.inventoryProduct.productId = productId;
    this.dataModal.inventoryProduct.cantidad = cantidad;
    if (this.productForm.invalid) {
      this.productForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    this.responseData.emit(this.dataModal);
  }

}
