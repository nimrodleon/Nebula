import {Component, EventEmitter, Input, OnInit, Output} from "@angular/core";
import {faBalanceScale, faBox, faCoins, faPlus, faWarehouse} from "@fortawesome/free-solid-svg-icons";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {Warehouse} from "app/account/company/interfaces";
import {WarehouseService} from "app/account/company/services";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {NgClass, NgForOf} from "@angular/common";
import {FormType, select2Productos, toastError} from "app/common/interfaces";
import {ItemRepairOrder, ItemRepairOrderDataModal} from "../../interfaces";
import _ from "lodash";

declare const jQuery: any;

@Component({
  selector: "app-material-repair-order-modal",
  standalone: true,
  imports: [
    FaIconComponent,
    ReactiveFormsModule,
    NgForOf,
    NgClass
  ],
  templateUrl: "./material-repair-order-modal.component.html"
})
export class MaterialRepairOrderModalComponent implements OnInit {
  protected readonly faBox = faBox;
  protected readonly faCoins = faCoins;
  protected readonly faBalanceScale = faBalanceScale;
  protected readonly faPlus = faPlus;
  protected readonly faWarehouse = faWarehouse;
  // ====================================================================================================
  @Input() itemRepairOrderDataModal: ItemRepairOrderDataModal = new ItemRepairOrderDataModal();
  @Output() responseData: EventEmitter<ItemRepairOrderDataModal> = new EventEmitter<ItemRepairOrderDataModal>();
  // ====================================================================================================
  warehouses: Warehouse[] = new Array<Warehouse>();
  formData: FormGroup = this.fb.group({
    warehouseId: ["", [Validators.required]],
    productId: ["", [Validators.required]],
    quantity: [null, [Validators.required]],
    monto: [null, [Validators.required]],
  });

  constructor(
    private fb: FormBuilder,
    private warehouseService: WarehouseService) {
  }

  ngOnInit(): void {
    // cargar la lista de productos.
    const productos = select2Productos("#productos",
      jQuery("#materialRepairOrderModal")).on("select2:select", (e: any) => {
      const data = e.params.data;
      this.itemRepairOrderDataModal.itemRepairOrder.description = data.description;
      this.itemRepairOrderDataModal.itemRepairOrder.precioUnitario = data.precioVentaUnitario;
      this.formData.controls["productId"].setValue(data.id);
      this.formData.controls["quantity"].setValue(1);
      this.formData.controls["monto"].setValue(data.precioVentaUnitario);
    });
    // cargar lista de almacenes.
    this.warehouseService.index().subscribe(result => this.warehouses = result);
    const myModal: any = document.querySelector("#materialRepairOrderModal");
    myModal.addEventListener("shown.bs.modal", () => {
      const {itemRepairOrder} = this.itemRepairOrderDataModal;
      if (this.itemRepairOrderDataModal.type === FormType.EDIT) {
        const newOption = new Option(itemRepairOrder.description, itemRepairOrder.productId, true, true);
        productos.append(newOption).trigger("change");
      }
      this.formData.controls["warehouseId"].setValue(itemRepairOrder.warehouseId);
      this.formData.controls["productId"].setValue(itemRepairOrder.productId);
      this.formData.controls["quantity"].setValue(itemRepairOrder.quantity);
      this.formData.controls["monto"].setValue(itemRepairOrder.monto);
    });
    myModal.addEventListener("hide.bs.modal", () => {
      this.formData.reset(new ItemRepairOrder());
      productos.val(null).trigger("change");
    });
  }

  // Verificar campo invalido.
  public inputIsInvalid(field: string) {
    return this.formData.controls[field].errors && this.formData.controls[field].touched;
  }

  public saveChanges(): void {
    if (this.formData.invalid) {
      this.formData.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    // Guardar datos, solo si es válido el formulario.
    const warehouseId = this.formData.get("warehouseId")?.value;
    const warehouse = _.find(this.warehouses, item => item.id === warehouseId);
    if (warehouse !== undefined) this.itemRepairOrderDataModal.itemRepairOrder.warehouseName = warehouse.name;
    this.itemRepairOrderDataModal.itemRepairOrder = {...this.itemRepairOrderDataModal.itemRepairOrder, ...this.formData.value};
    this.responseData.emit(this.itemRepairOrderDataModal);
  }

  public cambiarCantidad({value}: any): void {
    const monto: number = value * this.itemRepairOrderDataModal.itemRepairOrder.precioUnitario;
    this.formData.controls["monto"].setValue(monto);
  }

}
