import {Component, EventEmitter, Input, OnInit, Output} from "@angular/core";
import {faBasketShopping, faBox, faSignOutAlt} from "@fortawesome/free-solid-svg-icons";
import {ItemComprobanteModal} from "../../interfaces";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {EnumIdModal, select2Productos, toastError} from "app/common/interfaces";
import {Warehouse} from "app/account/company/interfaces";
import {WarehouseService} from "app/account/company/services";
import {Company} from "app/account/interfaces";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {NgClass, NgForOf} from "@angular/common";

@Component({
  selector: "app-add-product-modal",
  standalone: true,
  imports: [
    FaIconComponent,
    ReactiveFormsModule,
    NgClass,
    NgForOf
  ],
  templateUrl: "./add-product-modal.component.html"
})
export class AddProductModalComponent implements OnInit {
  faBox = faBox;
  faSignOutAlt = faSignOutAlt;
  faBasketShopping = faBasketShopping;
  @Input()
  company = new Company();
  @Input()
  itemComprobanteModal: ItemComprobanteModal = new ItemComprobanteModal();
  itemComprobanteForm: FormGroup = this.fb.group({
    ctdUnidadItem: [0, [Validators.required, Validators.min(1)]],
    productId: [null, [Validators.required]],
    warehouseId: [null, [Validators.required]],
  });
  @Output()
  responseData: EventEmitter<ItemComprobanteModal> = new EventEmitter<ItemComprobanteModal>();
  warehouses: Warehouse[] = new Array<Warehouse>();

  constructor(
    private fb: FormBuilder,
    private warehouseService: WarehouseService) {
  }

  ngOnInit(): void {
    const select2 = select2Productos("#producto",
      EnumIdModal.ID_ADD_PRODUCT_MODAL_SALES)
      .on("select2:select", (e: any) => {
        const data = e.params.data;
        this.itemComprobanteForm.controls["productId"].setValue(data.id.trim());
        this.itemComprobanteModal.itemComprobanteDto.mapProductToItem(data);
      });
    this.warehouseService.index().subscribe(result => this.warehouses = result);
    const myModal: any = document.querySelector(EnumIdModal.ID_ADD_PRODUCT_MODAL_SALES);
    myModal.addEventListener("shown.bs.modal", () => {
      if (this.itemComprobanteModal.typeOper === "EDIT") {
        this.itemComprobanteForm.reset({
          ctdUnidadItem: this.itemComprobanteModal.itemComprobanteDto.ctdUnidadItem,
          productId: this.itemComprobanteModal.itemComprobanteDto.productId,
          warehouseId: this.itemComprobanteModal.itemComprobanteDto.warehouseId,
        });
        const newOption = new Option(this.itemComprobanteModal.itemComprobanteDto.description, this.itemComprobanteModal.itemComprobanteDto.productId, true, true);
        select2.append(newOption).trigger("change");
      }
    });
    myModal.addEventListener("hidden.bs.modal", () => {
      this.itemComprobanteForm.reset({
        ctdUnidadItem: 0, productId: null, warehouseId: null,
      });
      select2.val(null).trigger("change");
    });
  }

  public inputIsInvalid(field: string) {
    return this.itemComprobanteForm.controls[field].errors
      && this.itemComprobanteForm.controls[field].touched;
  }

  public saveChanges(): void {
    if (this.itemComprobanteForm.invalid) {
      this.itemComprobanteForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    this.itemComprobanteModal.itemComprobanteDto = {
      ...this.itemComprobanteModal.itemComprobanteDto,
      ...this.itemComprobanteForm.value
    };
    this.itemComprobanteModal.itemComprobanteDto.recordType = "PRODUCTO";
    this.responseData.emit(this.itemComprobanteModal);
  }

}
