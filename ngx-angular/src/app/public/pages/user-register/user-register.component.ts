import {Component} from "@angular/core";
import {UserService} from "../../../account/user/services";
import {Router, RouterLink} from "@angular/router";
import {faEnvelope, faLock, faUser, faUserPlus} from "@fortawesome/free-solid-svg-icons";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import Swal from "sweetalert2";
import {catchError} from "rxjs/operators";
import {NgClass} from "@angular/common";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {LoaderComponent} from "app/common/loader/loader.component";
import {toastError} from "../../../common/interfaces";

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
  userForm: FormGroup = this.fb.group({
    userName: ["", Validators.required],
    email: ["", [Validators.required, Validators.email]],
    password: ["", [Validators.required, Validators.minLength(6)]]
  });
  loading: boolean = false;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private userService: UserService,) {
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
    this.loading = true;
    this.userService.register({...this.userForm.value})
      .pipe(catchError(err => {
        this.loading = false;
        throw err;
      })).subscribe(result => {
      if (result.ok) {
        this.loading = false;
        Swal.fire({
          icon: "success",
          title: "¡Registro Exitoso!",
          text: "Su registro ha sido exitoso. Por favor, revise su correo electrónico para confirmar su dirección de correo.",
          confirmButtonText: "OK"
        }).then(result => {
          if (result.isConfirmed) {
            this.router.navigate(["/login"]);
          }
        });
      } else {
        this.loading = false;
        Swal.fire({
          icon: "error",
          title: "Error",
          text: "Hubo un problema durante el registro. Por favor, inténtelo de nuevo más tarde.",
          confirmButtonText: "OK"
        });
      }
    });
  }

}
