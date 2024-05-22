import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserDataModal, UserTypeSystem } from '../../interfaces';
import { UserService } from '../../services';
import { FormType, toastError } from 'app/common/interfaces';

@Component({
  selector: 'app-user-modal',
  standalone: true,
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './user-modal.component.html'
})
export class UserModalComponent implements OnInit {
  private fb: FormBuilder = inject(FormBuilder);
  private userService: UserService = inject(UserService);
  @Input()
  data = new UserDataModal();
  @Output()
  responseData = new EventEmitter<UserDataModal>();
  userForm: FormGroup = this.fb.group({
    userName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    userType: [UserTypeSystem.Customer, Validators.required],
    disabled: [false, Validators.required],
  });

  ngOnInit(): void {
    const myModal: any = document.querySelector("#userModal");
    myModal.addEventListener("shown.bs.modal", () => {
      if (this.data.type === FormType.EDIT) {
        this.userForm.reset({
          userName: this.data.user.userName,
          email: this.data.user.email,
          userType: this.data.user.userType,
          disabled: this.data.user.disabled,
        });
      }
    });
    myModal.addEventListener("hide.bs.modal", () => {
      this.userForm.reset({
        userName: "",
        email: "",
        userType: UserTypeSystem.Customer,
        disabled: false,
      });
    });
  }

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
    // Guardar datos, sólo si es válido el formulario.
    const {
      userName,
      email,
      userType,
      disabled,
    } = this.userForm.value;
    if (this.data.type === FormType.ADD) {
      this.userService.registerFromAdmin({
        ...this.data.user,
        userName: userName || "",
        email: email || "",
        userType: userType || UserTypeSystem.Customer,
        disabled: disabled || false,
      }).subscribe(result => {
        this.data.user = result;
        this.responseData.emit(this.data);
      });
    }
    if (this.data.type === FormType.EDIT) {
      const { id } = this.data.user;
      this.userService.updateFromAdmin(id, {
        ...this.data.user,
        userName: userName || "",
        email: email || "",
        userType: userType || UserTypeSystem.Customer,
        disabled: disabled || false,
      }).subscribe(result => {
        this.data.user = result;
        this.responseData.emit(this.data);
      });
    }
  }

}
