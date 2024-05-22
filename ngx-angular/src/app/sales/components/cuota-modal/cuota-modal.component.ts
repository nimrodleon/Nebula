import {Component, EventEmitter, Input, OnInit, Output} from "@angular/core";
import {faCircleChevronRight} from "@fortawesome/free-solid-svg-icons";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import moment from "moment";
import {NgClass} from "@angular/common";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {CuotaDataModal, CuotaPagoDto} from "../../interfaces";

@Component({
  selector: "app-cuota-modal",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgClass,
    FaIconComponent
  ],
  templateUrl: "./cuota-modal.component.html"
})
export class CuotaModalComponent implements OnInit {
  faCircleChevronRight = faCircleChevronRight;
  @Input()
  cuotaDataModal = new CuotaDataModal();
  cuotaForm: FormGroup = this.fb.group({
    "uuid": ["-", [Validators.required]],
    "mtoCuotaPago": [0.00, [Validators.required, Validators.min(1)]],
    "fecCuotaPago": [moment().format("YYYY-MM-DD"), [Validators.required]]
  });
  @Output()
  responseData = new EventEmitter<CuotaDataModal>();

  constructor(private fb: FormBuilder) {
  }

  ngOnInit(): void {
    const myModalEl: any = document.querySelector("#cuota-modal");
    myModalEl.addEventListener("shown.bs.modal", () => {
      if (this.cuotaDataModal.type === "EDIT") {
        this.cuotaForm.reset(this.cuotaDataModal.cuotaPagoDto);
      }
    });
    myModalEl.addEventListener("hidden.bs.modal", () => {
      this.cuotaForm.reset(new CuotaPagoDto());
    });
  }

  public inputIsInvalid(field: string) {
    return this.cuotaForm.controls[field].errors && this.cuotaForm.controls[field].touched;
  }

  public saveChanges(): void {
    if (this.cuotaForm.invalid) {
      this.cuotaForm.markAllAsTouched();
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    this.cuotaDataModal.cuotaPagoDto = {...this.cuotaForm.value};
    this.responseData.emit(this.cuotaDataModal);
  }

}
