import {Component} from "@angular/core";
import {UserBusinessService} from "app/account/user/services";
import {Router, RouterLink} from "@angular/router";
import {faEnvelope, faIdCard, faLock, faSquarePhone, faUser, faUserPlus} from "@fortawesome/free-solid-svg-icons";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import Swal from "sweetalert2";
import {NgClass} from "@angular/common";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {LoaderComponent} from "app/common/loader/loader.component";
import {toastError} from "app/common/interfaces";

@Component({
  selector: "app-user-register",
  templateUrl: "./user-register.component.html",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgClass,
    FaIconComponent,
    RouterLink,
    LoaderComponent
  ],
  styleUrls: ["./user-register.component.scss"]
})
export class UserRegisterComponent {
  faUser = faUser;
  faEnvelope = faEnvelope;
  faLock = faLock;
  faUserPlus = faUserPlus;
  faIdCard = faIdCard;
  faSquarePhone = faSquarePhone;
  userForm: FormGroup = this.fb.group({
    userName: ["", Validators.required],
    email: ["", [Validators.required, Validators.email]],
    password: ["", [Validators.required, Validators.minLength(6)]],
    fullName: ["", Validators.required],
    phoneNumber: ["", Validators.required],
  });

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private userBusinessService: UserBusinessService,) {
  }

  // Verificar campo invalido.
  public inputIsInvalid(field: string) {
    return this.userForm.controls[field].errors
      && this.userForm.controls[field].touched;
  }

  async registerUser(): Promise<void> {
    if (this.userForm.invalid) {
      this.userForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    this.userBusinessService.create({...this.userForm.value})
      .subscribe(result => {
        Swal.fire({
          icon: "success",
          title: "¡Registro Exitoso!",
          text: "¡Su registro ha sido exitoso! Por favor, inicie sesión en la siguiente pantalla.",
          confirmButtonText: "OK"
        }).then(result => {
          if (result.isConfirmed) {
            this.router.navigate(["/login"]);
          }
        });
      });
  }
}
