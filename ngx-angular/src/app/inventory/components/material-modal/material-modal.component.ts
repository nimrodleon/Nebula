import {Component, EventEmitter, Input, OnInit, Output} from "@angular/core";
import {faBox, faTags, faWarehouse} from "@fortawesome/free-solid-svg-icons";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {Warehouse} from "app/account/company/interfaces";
import {WarehouseService} from "app/account/company/services";
import {select2Productos, toastError} from "app/common/interfaces";
import {MaterialDataModal} from "../../interfaces";
import _ from "lodash";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {NgClass, NgForOf} from "@angular/common";

declare const jQuery: any;

@Component({
  selector: "app-material-modal",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FaIconComponent,
    NgClass,
    NgForOf
  ],
  templateUrl: "./material-modal.component.html"
})
export class MaterialModalComponent implements OnInit {
  faBox = faBox;
  faWarehouse = faWarehouse;
  faTags = faTags;
  warehouses: Array<Warehouse> = new Array<Warehouse>();
  @Input()
  dataModal = new MaterialDataModal();
  productForm: FormGroup = this.fb.group({
    productId: [null, [Validators.required]],
    warehouseId: [null, [Validators.required]],
    cantidad: [null, [Validators.required]],
  });
  @Output()
  responseData = new EventEmitter<MaterialDataModal>();

  constructor(
    private fb: FormBuilder,
    private warehouseService: WarehouseService) {
  }

  ngOnInit(): void {
    const productos = select2Productos("#producto",
      jQuery("#material-modal"))
      .on("select2:select", (e: any) => {
        const data = e.params.data;
        this.productForm.controls["productId"].setValue(data.id);
        this.dataModal.materialDetail.productName = data.description;
      });
    this.warehouseService.index()
      .subscribe(result => this.warehouses = result);
    const myModal: any = document.querySelector("#material-modal");
    myModal.addEventListener("shown.bs.modal", () => {
      if (this.dataModal.type === "EDIT") {
        this.productForm.reset({
          productId: this.dataModal.materialDetail.productId,
          warehouseId: this.dataModal.materialDetail.warehouseId,
          cantidad: this.dataModal.materialDetail.cantSalida,
        });
        const {productId, productName} = this.dataModal.materialDetail;
        const newOption = new Option(productName, productId, true, true);
        productos.append(newOption).trigger("change");
      }
    });
    myModal.addEventListener("hidden.bs.modal", () => {
      this.productForm.reset({productId: null, warehouseId: null, cantidad: null});
      productos.val(null).trigger("change");
    });
  }

  public inputIsInvalid(field: string) {
    return this.productForm.controls[field].errors
      && this.productForm.controls[field].touched;
  }

  public saveChanges(): void {
    const warehouseId: string = this.productForm.get("warehouseId")?.value;
    const warehouse = _.find(this.warehouses, (o: Warehouse) => o.id === warehouseId);
    if (warehouse !== undefined) this.dataModal.materialDetail.warehouseName = warehouse.name;
    this.dataModal.materialDetail.productId = this.productForm.get("productId")?.value;
    this.dataModal.materialDetail.warehouseId = this.productForm.get("warehouseId")?.value;
    this.dataModal.materialDetail.cantSalida = this.productForm.get("cantidad")?.value;
    if (this.dataModal.type === "EDIT") {
      const {cantSalida, cantRetorno} = this.dataModal.materialDetail;
      this.dataModal.materialDetail.cantUsado = cantSalida - cantRetorno;
    }
    if (this.productForm.invalid) {
      this.productForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    this.responseData.emit(this.dataModal);
  }

}
