import {Component, EventEmitter, Input, OnInit, Output} from "@angular/core";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {ItemComprobanteDto} from "app/cashier/quicksale/interfaces";
import {ItemComprobanteModal} from "../../interfaces";
import {EnumIdModal, toastError} from "app/common/interfaces";
import {Company} from "app/account/interfaces";
import {v4 as uuid} from "uuid";
import {DecimalPipe, NgClass} from "@angular/common";

@Component({
  selector: "app-add-entrada-modal",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgClass,
    DecimalPipe
  ],
  templateUrl: "./add-entrada-modal.component.html"
})
export class AddEntradaModalComponent implements OnInit {
  @Input()
  company = new Company();
  @Input()
  itemComprobanteModal: ItemComprobanteModal = new ItemComprobanteModal();
  itemComprobanteForm: FormGroup = this.fb.group({
    tipoItem: ["BIEN", [Validators.required]],
    ctdUnidadItem: [0, [Validators.required, Validators.min(1)]],
    codUnidadMedida: ["NIU:UNIDAD (BIENES)", [Validators.required]],
    description: ["", [Validators.required]],
    mtoValorVentaItem: [0.0, [Validators.required, Validators.min(0)]],
    igvSunat: ["GRAVADO", [Validators.required]],
    mtoPrecioVentaUnitario: [0.0, [Validators.required]],
    mtoTotalItem: [0.0, [Validators.required, Validators.min(0)]],
  });
  @Output()
  responseData: EventEmitter<ItemComprobanteModal> = new EventEmitter<ItemComprobanteModal>();

  constructor(
    private fb: FormBuilder) {
  }

  ngOnInit(): void {
    const myModal: any = document.querySelector(EnumIdModal.ID_ADD_ENTRADA_MANUAL_SALES);
    myModal.addEventListener("shown.bs.modal", () => {
      if (this.itemComprobanteModal.typeOper === "EDIT") {
        const itemComprobante: ItemComprobanteDto = this.itemComprobanteModal.itemComprobanteDto;
        this.itemComprobanteForm.reset({
          tipoItem: itemComprobante.tipoItem,
          ctdUnidadItem: itemComprobante.ctdUnidadItem,
          codUnidadMedida: itemComprobante.codUnidadMedida,
          description: itemComprobante.description,
          mtoValorVentaItem: itemComprobante.getValorVentaItem(this.company),
          igvSunat: itemComprobante.igvSunat,
          mtoPrecioVentaUnitario: itemComprobante.mtoPrecioVentaUnitario,
          mtoTotalItem: itemComprobante.getImporteTotal(),
        });
      }
    });
    myModal.addEventListener("hidden.bs.modal", () => {
      this.itemComprobanteForm.reset({
        tipoItem: "BIEN",
        ctdUnidadItem: 0,
        codUnidadMedida: "NIU:UNIDAD (BIENES)",
        description: "",
        mtoValorVentaItem: 0.0,
        igvSunat: "GRAVADO",
        mtoPrecioVentaUnitario: 0.0,
        mtoTotalItem: 0.0,
      });
    });
  }

  private calcularMontos(importeItem: number, cantidad: number): void {
    const igvSunat = this.itemComprobanteForm.get("igvSunat")?.value;
    const porcentajeIgv: number = igvSunat !== "GRAVADO" ? 1 : (this.company.porcentajeIgv / 100) + 1;
    this.itemComprobanteForm.controls["mtoValorVentaItem"].setValue(importeItem / porcentajeIgv);
    this.itemComprobanteForm.controls["mtoPrecioVentaUnitario"].setValue(importeItem / cantidad);
  }

  public cambiarCantidadItem({value}: any): void {
    const mtoTotalItem: number = this.itemComprobanteForm.get("mtoTotalItem")?.value;
    this.calcularMontos(mtoTotalItem, value);
  }

  public cambiarTipoDeOperacion({value}: any): void {
    const ctdUnidadItem: number = this.itemComprobanteForm.get("ctdUnidadItem")?.value;
    const mtoTotalItem: number = this.itemComprobanteForm.get("mtoTotalItem")?.value;
    this.calcularMontos(mtoTotalItem, ctdUnidadItem);
  }

  public cambiarMtoTotalItem({value}: any): void {
    const ctdUnidadItem: number = this.itemComprobanteForm.get("ctdUnidadItem")?.value;
    this.calcularMontos(value, ctdUnidadItem);
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
    if (this.itemComprobanteModal.typeOper === "ADD") {
      this.itemComprobanteModal.itemComprobanteDto.productId = uuid();
      this.itemComprobanteModal.itemComprobanteDto.warehouseId = uuid();
    }
    this.itemComprobanteModal.itemComprobanteDto.recordType = "ENTRADA";
    this.responseData.emit(this.itemComprobanteModal);
  }

}
