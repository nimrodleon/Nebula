import {Component, EventEmitter, Input, OnInit, Output} from "@angular/core";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {select2Contactos, toastError} from "app/common/interfaces";
import {ReceivableDataModal} from "app/receivable/interfaces";
import {NgClass} from "@angular/common";

@Component({
  selector: "app-cargo-modal",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: "./cargo-modal.component.html"
})
export class CargoModalComponent implements OnInit {
  @Input()
  cargoDataModal: ReceivableDataModal = new ReceivableDataModal();
  cargoForm: FormGroup = this.fb.group({
    endDate: [null, [Validators.required]],
    contactId: [null, [Validators.required]],
    cargo: [null, [Validators.required]],
    remark: [null, [Validators.required]]
  });
  @Output()
  responseData = new EventEmitter<ReceivableDataModal>();

  constructor(private fb: FormBuilder) {
  }

  ngOnInit(): void {
    const contacto = select2Contactos("#contact", "#cargo-modal")
      .on("select2:select", ({params}: any) => {
        this.cargoDataModal.receivable.contactId = params.data.id;
        this.cargoDataModal.receivable.contactName = params.data.name;
        this.cargoForm.controls["contactId"].setValue(params.data.id);
      });
    const myModal: any = document.querySelector("#cargo-modal");
    myModal.addEventListener("shown.bs.modal", () => {
      this.cargoForm.reset({...this.cargoDataModal.receivable});
      if (this.cargoDataModal.type === "EDIT") {
        const {contactId, contactName} = this.cargoDataModal.receivable;
        const newOption = new Option(contactName, contactId, true, true);
        contacto.append(newOption).trigger("change");
      }
    });
    myModal.addEventListener("hide.bs.modal", () => {
      this.cargoForm.reset({endDate: null, contactId: null, cargo: null, remark: null});
      contacto.val(null).trigger("change");
    });
  }

  public validarCampo(campo: string) {
    return this.cargoForm.controls[campo].errors && this.cargoForm.controls[campo].touched;
  }

  public saveChanges(): void {
    if (this.cargoForm.invalid) {
      this.cargoForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    // this.cargoDataModal.receivable.document = '-';
    if (this.cargoDataModal.type === "ADD") this.cargoDataModal.receivable.status = "PENDIENTE";
    this.cargoDataModal.receivable.endDate = this.cargoForm.get("endDate")?.value;
    this.cargoDataModal.receivable.cargo = this.cargoForm.get("cargo")?.value;
    this.cargoDataModal.receivable.remark = this.cargoForm.get("remark")?.value;
    this.responseData.emit(this.cargoDataModal);
  }

}
