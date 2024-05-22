import {Component, EventEmitter, Input, OnInit, Output} from "@angular/core";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {Warehouse, WarehouseDataModal} from "../../interfaces";
import {NgClass} from "@angular/common";
import {toastError} from "../../../../common/interfaces";

@Component({
  selector: "app-warehouse-modal",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: "./warehouse-modal.component.html"
})
export class WarehouseModalComponent implements OnInit {
  @Input()
  warehouseDataModal = new WarehouseDataModal();
  @Output()
  responseData = new EventEmitter<WarehouseDataModal>();
  warehouseForm: FormGroup = this.fb.group({
    id: [null],
    name: ["", [Validators.required]],
    remark: ["", [Validators.required]]
  });

  constructor(
    private fb: FormBuilder) {
  }

  ngOnInit(): void {
    // cargar valores por defecto.
    const myModal: any = document.querySelector("#warehouse-modal");
    myModal.addEventListener("shown.bs.modal", () => {
      this.warehouseForm.reset(this.warehouseDataModal.warehouse);
    });
    myModal.addEventListener("hide.bs.modal", () => {
      this.warehouseForm.reset(new Warehouse());
    });
  }

  // Verificar campo invalido.
  public inputIsInvalid(field: string) {
    return this.warehouseForm.controls[field].errors
      && this.warehouseForm.controls[field].touched;
  }

  // guardar los cambios establecidos.
  public saveChanges(): void {
    if (this.warehouseForm.invalid) {
      this.warehouseForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    const {id} = this.warehouseDataModal.warehouse;
    this.warehouseDataModal.warehouse = {...this.warehouseForm.value, id};
    this.responseData.emit(this.warehouseDataModal);
  }

}
