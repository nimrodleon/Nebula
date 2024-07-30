import {Component, EventEmitter, Input, OnInit, Output, inject} from "@angular/core";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {UserDataModal, UserRole} from "../../interfaces";
import {UserPersonalService} from "../../services";
import {FormType, toastError} from "app/common/interfaces";
import {NgClass} from "@angular/common";

@Component({
  selector: "app-user-modal",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: "./user-modal.component.html"
})
export class UserModalComponent implements OnInit {
  private fb: FormBuilder = inject(FormBuilder);
  private userPersonalService: UserPersonalService = inject(UserPersonalService);
  @Input()
  data = new UserDataModal();
  @Output()
  responseData = new EventEmitter<UserDataModal>();
  userForm: FormGroup = this.fb.group({
    userName: ["", [Validators.required]],
    email: ["", [Validators.required, Validators.email]],
    userRole: [UserRole.User, [Validators.required]],
    fullName: ["", [Validators.required]],
    phoneNumber: ["", [Validators.required]],
  });

  ngOnInit(): void {
    const myModal: any = document.querySelector("#userModal");
    myModal.addEventListener("shown.bs.modal", () => {
      if (this.data.type === FormType.EDIT) {
        this.userForm.reset({
          userName: this.data.user.userName,
          email: this.data.user.email,
          userRole: this.data.user.userRole,
          fullName: this.data.user.fullName,
          phoneNumber: this.data.user.phoneNumber,
        });
      }
    });
    myModal.addEventListener("hide.bs.modal", () => {
      this.userForm.reset({
        userName: "",
        email: "",
        userRole: UserRole.User,
        fullName: "",
        phoneNumber: "",
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
    if (this.data.type === FormType.ADD) {
      this.userPersonalService.create({
        ...this.userForm.value
      }).subscribe(result => {
        this.data.user = result;
        this.responseData.emit(this.data);
      });
    }
    if (this.data.type === FormType.EDIT) {
      const {id} = this.data.user;
      this.userPersonalService.update(id, {
        ...this.userForm.value
      }).subscribe(result => {
        this.data.user = result;
        this.responseData.emit(this.data);
      });
    }
  }

}
