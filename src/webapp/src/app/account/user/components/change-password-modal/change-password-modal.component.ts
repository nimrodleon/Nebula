import {Component, EventEmitter, inject, Input, Output} from "@angular/core";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {NgClass} from "@angular/common";
import {UserPersonalService} from "../../services";
import {toastError, toastSuccess} from "app/common/interfaces";
import {UserDataModal} from "../../interfaces";

@Component({
  selector: "app-change-password-modal",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: "./change-password-modal.component.html",
})
export class ChangePasswordModalComponent {
  @Input()
  data = new UserDataModal();
  private fb: FormBuilder = inject(FormBuilder);
  private userPersonalService: UserPersonalService = inject(UserPersonalService);
  @Output()
  responseData = new EventEmitter<boolean>();

  userForm: FormGroup = this.fb.group({
    password: ["", [Validators.required]],
  });

  public inputIsInvalid(field: string) {
    const control = this.userForm.get(field);
    return control && control.errors && control.touched;
  }

  public saveChanges(): void {
    if (this.userForm.invalid) {
      this.userForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    const {id} = this.data.user;
    const {password} = this.userForm.value;
    this.userPersonalService.changePassword(id, password)
      .subscribe(() => {
        this.userForm.reset();
        toastSuccess("Contraseña actualizada correctamente!");
        this.responseData.emit(true);
      });
  }

}
