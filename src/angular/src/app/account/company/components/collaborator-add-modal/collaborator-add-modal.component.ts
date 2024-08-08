import {Component, EventEmitter, Input, OnInit, Output} from "@angular/core";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {CompanyRoles} from "app/account/user/interfaces";
import {InviteCollaborator} from "../../interfaces";
import {NgClass} from "@angular/common";
import {toastError} from "../../../../common/interfaces";

@Component({
  selector: "app-collaborator-add-modal",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: "./collaborator-add-modal.component.html"
})
export class CollaboratorAddModalComponent implements OnInit {
  @Input()
  companyId: string = "";
  usuarioForm: FormGroup;
  @Output()
  responseData: EventEmitter<InviteCollaborator> = new EventEmitter<InviteCollaborator>();

  constructor(
    private fb: FormBuilder,) {
    this.usuarioForm = this.fb.group({
      email: ["", [Validators.required, Validators.email]],
      userRole: [CompanyRoles.User, [Validators.required]],
    });
  }

  ngOnInit(): void {
    const myModal: any = document.querySelector("#collaborator-add-modal");
    myModal.addEventListener("hide.bs.modal", () => {
      this.usuarioForm.reset({
        email: "",
        userRole: CompanyRoles.User
      });
    });
  }

  // Verificar campo invalido.
  public inputIsInvalid(field: string) {
    return this.usuarioForm.controls[field].errors && this.usuarioForm.controls[field].touched;
  }

  public guardarUsuario(): void {
    if (this.usuarioForm.invalid) {
      this.usuarioForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    const inviteCollaborator: InviteCollaborator = {...this.usuarioForm.value};
    inviteCollaborator.companyId = this.companyId.trim();
    this.responseData.emit(inviteCollaborator);
  }

}
