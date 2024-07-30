import {Component, EventEmitter, Output} from "@angular/core";
import {UserDataService} from "../../user-data.service";
import {NgClass, NgForOf} from "@angular/common";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {toastError, toastSuccess} from "../../interfaces";
import {AuthService} from "app/account/user/services";

@Component({
  selector: "app-cambiar-local",
  standalone: true,
  imports: [
    NgForOf,
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: "./cambiar-local.component.html",
})
export class CambiarLocalComponent {
  companyForm: FormGroup = this.fb.group({
    idLocal: [this.localDefault, [Validators.required]],
  });
  @Output()
  closeModalEvent = new EventEmitter();

  constructor(
    private fb: FormBuilder,
    private userDataService: UserDataService,
    private authService: AuthService) {
  }

  public get localDefault(): number {
    return this.userDataService.localDefault;
  }

  public get locales() {
    return this.userDataService.locales;
  }

  public inputIsInvalid(field: string) {
    const control = this.companyForm.get(field);
    return control && control.errors && control.touched;
  }

  public saveChanges(): void {
    if (this.companyForm.invalid) {
      this.companyForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    const {companyId} = this.companyForm.value;
    this.authService.localChange(companyId)
      .subscribe((res: any) => {
        localStorage.setItem("token", res.token);
        this.userDataService.cargarData();
        toastSuccess("Local cambiado correctamente!");
        this.closeModalEvent.emit();
      });
  }

}
