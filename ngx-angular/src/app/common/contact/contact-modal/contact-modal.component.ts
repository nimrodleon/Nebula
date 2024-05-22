import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { faSearch } from "@fortawesome/free-solid-svg-icons";
import { catchError } from "rxjs/operators";
import { throwError } from "rxjs";
import { NgClass } from "@angular/common";
import { FaIconComponent } from "@fortawesome/angular-fontawesome";
import { Contact, ContactDataModal } from "app/contact/interfaces";
import { ContactService } from "app/contact/services";
import { toastError } from "../../interfaces";

@Component({
  selector: "app-contact-modal",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgClass,
    FaIconComponent
  ],
  templateUrl: "./contact-modal.component.html"
})
export class ContactModalComponent implements OnInit {
  faSearch = faSearch;
  @Input()
  contactDataModal = new ContactDataModal();
  @Output()
  responseData = new EventEmitter<ContactDataModal>();
  // ====================================================================================================
  contactForm: FormGroup = this.fb.group({
    id: [null],
    document: ["", [Validators.required]],
    docType: [0, [Validators.required]],
    name: ["", [Validators.required]],
    address: ["", [Validators.required]],
    phoneNumber: ["", [Validators.required]],
    codUbigeo: ["-", [Validators.required]],
  });

  constructor(
    private fb: FormBuilder,
    private contactService: ContactService) {
  }

  ngOnInit(): void {
    const myModal: any = document.querySelector("#contact-modal");
    myModal.addEventListener("shown.bs.modal", () => {
      this.contactForm.reset(this.contactDataModal.contact);
    });
    myModal.addEventListener("hide.bs.modal", () => {
      this.contactForm.reset(new Contact());
    });
  }

  // Verificar campo invalido.
  public inputIsInvalid(field: string) {
    return this.contactForm.controls[field].errors
      && this.contactForm.controls[field].touched;
  }

  // guardar cambios del formulario.
  public saveChanges(): void {
    if (this.contactForm.invalid) {
      this.contactForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    const { id } = this.contactDataModal.contact;
    this.contactDataModal.contact = { ...this.contactForm.value, id };
    this.responseData.emit(this.contactDataModal);
  }

  public buscarContactoEnPadronContribuyentes(): void {
    const { document, docType } = this.contactForm.value;
    if (docType === "0:SIN DEFINIR" || docType === "7:PASAPORTE") {
      toastError("Seleccione DNI y/o RUC!");
      return;
    }
    this.contactService.getContribuyente(document.trim())
      .pipe(catchError(() => {
        this.contactForm.markAllAsTouched();
        return throwError(() => new Error("El recurso solicitado no se encuentra disponible"));
      })).subscribe(result => {
        let document: string = "";
        if (docType === "1:D.N.I") document = result.dni;
        if (docType === "6:R.U.C") document = result.ruc;
        this.contactForm.controls["document"].setValue(document);
        this.contactForm.controls["name"].setValue(result.nombre);
        let address = "-";
        if (result.tipo_via !== "-" && result.nombre_via !== "-" && result.numero !== "-")
          address = `${result.tipo_via} ${result.nombre_via} NRO. ${result.numero}`;
        if (address.trim() !== "") this.contactForm.controls["address"].setValue(address);
        if (result.ubigeo)
          this.contactForm.controls["codUbigeo"].setValue(result.ubigeo);
      });
  }

}
