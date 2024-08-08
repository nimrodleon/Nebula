import {Component, EventEmitter, Input, OnInit, Output} from "@angular/core";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {InvoiceSerie, InvoiceSerieDataModal, Warehouse} from "../../interfaces";
import {WarehouseService} from "../../services";
import _ from "lodash";
import {NgClass, NgForOf} from "@angular/common";
import {toastError} from "../../../../common/interfaces";

@Component({
  selector: "app-invoice-serie-modal",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgClass,
    NgForOf
  ],
  templateUrl: "./invoice-serie-modal.component.html"
})
export class InvoiceSerieModalComponent implements OnInit {
  @Input()
  companyId: string = "";
  @Input()
  invoiceSerieDataModal: InvoiceSerieDataModal = new InvoiceSerieDataModal();
  @Output()
  responseData = new EventEmitter<InvoiceSerieDataModal>();
  warehouses: Array<Warehouse> = new Array<Warehouse>();
  invoiceSerieForm: FormGroup = this.fb.group({
    name: ["", [Validators.required]],
    warehouseId: [null, [Validators.required]],
    warehouseName: [null],
    notaDeVenta: ["", [Validators.required]],
    counterNotaDeVenta: [0, [Validators.required, Validators.min(0)]],
    boleta: ["", [Validators.required]],
    counterBoleta: [0, [Validators.required, Validators.min(0)]],
    factura: ["", [Validators.required]],
    counterFactura: [0, [Validators.required, Validators.min(0)]],
    creditNoteBoleta: ["", [Validators.required]],
    counterCreditNoteBoleta: [0, [Validators.required, Validators.min(0)]],
    creditNoteFactura: ["", [Validators.required]],
    counterCreditNoteFactura: [0, [Validators.required, Validators.min(0)]]
  });

  constructor(
    private fb: FormBuilder,
    private warehouseService: WarehouseService) {
  }

  ngOnInit(): void {
    // cargar lista de almacenes.
    this.warehouseService.index("", this.companyId)
      .subscribe(result => this.warehouses = result);
    // cargar valores por defecto.
    const myModal: any = document.querySelector("#invoice-serie-modal");
    myModal.addEventListener("shown.bs.modal", () => {
      if (this.invoiceSerieDataModal.type === "EDIT") {
        this.invoiceSerieForm.reset(this.invoiceSerieDataModal.invoiceSerie);
      }
    });
    myModal.addEventListener("hidden.bs.modal", () => {
      this.invoiceSerieForm.reset(new InvoiceSerie());
    });
  }

  // Verificar campo invalido.
  public inputIsInvalid(field: string) {
    return this.invoiceSerieForm.controls[field].errors
      && this.invoiceSerieForm.controls[field].touched;
  }

  // guardar cambios.
  public saveChanges(): void {
    if (this.invoiceSerieForm.invalid) {
      this.invoiceSerieForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    const warehouseId = this.invoiceSerieForm.get("warehouseId")?.value;
    const warehouse = _.find(this.warehouses, (o: Warehouse) => o.id === warehouseId);
    if (warehouse !== undefined) this.invoiceSerieForm.controls["warehouseName"].setValue(warehouse.name);
    // Guardar datos, sólo si es válido el formulario.
    const {id} = this.invoiceSerieDataModal.invoiceSerie;
    this.invoiceSerieDataModal.invoiceSerie = this.invoiceSerieForm.value;
    if (this.invoiceSerieDataModal.type === "EDIT") this.invoiceSerieDataModal.invoiceSerie.id = id;
    this.responseData.emit(this.invoiceSerieDataModal);
  }

}
