import {Component, EventEmitter, Input, OnInit, Output} from "@angular/core";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {CajaDiaria} from "app/cashier/interfaces";
import {CajaDiariaService} from "app/cashier/services";
import {Receivable} from "../../interfaces";
import {ReceivableService} from "../../services";
import moment from "moment";
import _ from "lodash";
import {NgClass, NgForOf} from "@angular/common";
import {toastError} from "../../../common/interfaces";

@Component({
  selector: "app-abono-modal",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgForOf,
    NgClass
  ],
  templateUrl: "./abono-modal.component.html"
})
export class AbonoModalComponent implements OnInit {
  @Input()
  cargoId: string = "-";
  cajaDiarias: Array<CajaDiaria> = new Array<CajaDiaria>();
  abonoForm: FormGroup = this.fb.group({
    cajaDiaria: ["-"],
    formaPago: [null, [Validators.required]],
    abono: [null, [Validators.required]],
    remark: [null, [Validators.required]]
  });
  @Output()
  responseData = new EventEmitter<Receivable>();

  constructor(
    private fb: FormBuilder,
    private cajaDiariaService: CajaDiariaService,
    private receivableService: ReceivableService) {
  }

  ngOnInit(): void {
    this.cajaDiariaService.cajasAbiertas()
      .subscribe(result => this.cajaDiarias = result);
    const myModal: any = document.querySelector("#abono-modal");
    myModal.addEventListener("shown.bs.modal", () => {
      this.abonoForm.reset({cajaDiaria: "-", formaPago: null, abono: null, remark: null});
    });
    myModal.addEventListener("hide.bs.modal", () => {
      this.abonoForm.reset({cajaDiaria: "-", formaPago: null, abono: null, remark: null});
    });
  }

  public validarCampo(campo: string) {
    return this.abonoForm.controls[campo].errors && this.abonoForm.controls[campo].touched;
  }

  public saveChanges(): void {
    if (this.abonoForm.invalid) {
      this.abonoForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    const abono: Receivable = new Receivable();
    abono.type = "ABONO";
    abono.status = "-";
    abono.cargo = 0;
    abono.document = "-";
    abono.cajaDiaria = this.abonoForm.get("cajaDiaria")?.value;
    const cajaDiaria = _.find(this.cajaDiarias, (item: CajaDiaria) => item.id === abono.cajaDiaria);
    if (cajaDiaria !== undefined) abono.terminal = cajaDiaria.terminal;
    abono.formaPago = this.abonoForm.get("formaPago")?.value;
    abono.abono = this.abonoForm.get("abono")?.value;
    abono.remark = this.abonoForm.get("remark")?.value;
    abono.receivableId = this.cargoId;
    abono.createdAt = moment().format("YYYY-MM-DD");
    abono.endDate = "-";
    this.receivableService.create(abono)
      .subscribe(result => this.responseData.emit(result));
  }

}
