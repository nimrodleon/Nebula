import {Component, EventEmitter, Input, OnInit, Output} from "@angular/core";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {select2Contactos} from "app/common/interfaces";
import {CashierDetailService} from "../../services";
import {CashierDetail} from "../../interfaces";
import {NgClass} from "@angular/common";

@Component({
  selector: "app-caja-chica-modal",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: "./caja-chica-modal.component.html"
})
export class CajaChicaModalComponent implements OnInit {
  @Input()
  cajaDiariaId: string = "";
  @Output()
  responseData = new EventEmitter<CashierDetail>();
  cashierDetail: CashierDetail = new CashierDetail();
  cajaChicaForm: FormGroup = this.fb.group({
    typeOperation: ["", [Validators.required]],
    contactId: [null, [Validators.required]],
    contactName: [null],
    amount: [null, [Validators.required, Validators.min(0.1)]],
    remark: ["", [Validators.required]],
  });

  constructor(
    private fb: FormBuilder,
    private cashierDetailService: CashierDetailService) {
  }

  ngOnInit(): void {
    const contactEl = select2Contactos("#contactId", "#caja-chica-modal")
      .on("select2:select", (e: any) => {
        const data = e.params.data;
        const text = data.text.split("-");
        const contactName = text[text.length - 1].trim();
        this.cajaChicaForm.controls["contactId"].setValue(data.id);
        this.cajaChicaForm.controls["contactName"].setValue(contactName);
      });
    // cargar valores por defecto.
    if (document.querySelector("#caja-chica-modal")) {
      const myModal: any = document.querySelector("#caja-chica-modal");
      myModal.addEventListener("hide.bs.modal", () => {
        contactEl.val(null).trigger("change");
        this.cashierDetail = new CashierDetail();
        this.cashierDetail.cajaDiariaId = this.cajaDiariaId;
        this.cajaChicaForm.reset();
      });
    }
  }

  // Verificar campo invalido.
  public inputIsInvalid(field: string) {
    return this.cajaChicaForm.controls[field].errors && this.cajaChicaForm.controls[field].touched;
  }

  // guardar cambios.
  public saveChanges(): void {
    if (this.cajaChicaForm.invalid) {
      this.cajaChicaForm.markAllAsTouched();
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    this.cashierDetail = {...this.cajaChicaForm.value};
    this.cashierDetail.cajaDiariaId = this.cajaDiariaId;
    this.cashierDetailService.create(this.cashierDetail)
      .subscribe(result => {
        this.responseData.emit(result);
      });
  }

}
