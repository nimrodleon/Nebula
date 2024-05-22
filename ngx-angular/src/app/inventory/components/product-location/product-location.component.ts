import {Component, EventEmitter, Input, OnInit, Output} from "@angular/core";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {faBox, faLock} from "@fortawesome/free-solid-svg-icons";
import {select2Productos, toastError} from "app/common/interfaces";
import {ProductLocationDataModal} from "../../interfaces";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {NgClass} from "@angular/common";

declare const jQuery: any;

@Component({
  selector: "app-product-location",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FaIconComponent,
    NgClass
  ],
  templateUrl: "./product-location.component.html"
})
export class ProductLocationComponent implements OnInit {
  faBox = faBox;
  faLock = faLock;
  @Input()
  dataModal = new ProductLocationDataModal();
  productForm: FormGroup = this.fb.group({
    productId: [null, [Validators.required]],
    quantityMax: [null, [Validators.required]],
    quantityMin: [null, [Validators.required]],
  });
  @Output()
  responseData = new EventEmitter<ProductLocationDataModal>();

  constructor(
    private fb: FormBuilder,) {
  }

  ngOnInit(): void {
    const productos = select2Productos("#producto",
      jQuery("#product-location"))
      .on("select2:select", (e: any) => {
        const data = e.params.data;
        this.productForm.controls["productId"].setValue(data.id);
        this.dataModal.locationDetail.productName = data.description;
        this.dataModal.locationDetail.barcode = data.barcode;
      });
    const myModal: any = document.querySelector("#product-location");
    myModal.addEventListener("shown.bs.modal", () => {
      if (this.dataModal.type === "ADD") {
        this.productForm.reset({productId: null, quantityMax: null, quantityMin: null});
        productos.val(null).trigger("change");
      }
      if (this.dataModal.type === "EDIT") {
        this.productForm.reset({...this.dataModal.locationDetail});
        const {productId, productName} = this.dataModal.locationDetail;
        const newOption = new Option(productName, productId, true, true);
        productos.append(newOption).trigger("change");
      }
    });
  }

  public inputIsInvalid(field: string) {
    return this.productForm.controls[field].errors && this.productForm.controls[field].touched;
  }

  public saveChanges(): void {
    this.dataModal.locationDetail = {...this.dataModal.locationDetail, ...this.productForm.value};
    if (this.productForm.invalid) {
      this.productForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    this.responseData.emit(this.dataModal);
  }

}
