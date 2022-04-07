import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {faBars} from '@fortawesome/free-solid-svg-icons';
import {User, UserDataModal, UserRegister} from '../../interfaces';
import {ResponseData} from 'src/app/global/interfaces';
import {UserService} from '../../services';

@Component({
  selector: 'app-user-modal',
  templateUrl: './user-modal.component.html'
})
export class UserModalComponent implements OnInit {
  @Input() data: UserDataModal = new UserDataModal();
  @Output() responseData = new EventEmitter<ResponseData<User>>();
  faBars = faBars;
  userForm: FormGroup = this.fb.group({
    userName: ['', [Validators.required]],
    email: ['', [Validators.required]],
    role: ['User', [Validators.required]]
  });

  constructor(
    private fb: FormBuilder,
    private userService: UserService) {
  }

  ngOnInit(): void {
    const myModal: any = document.querySelector('#user-modal');
    myModal.addEventListener('shown.bs.modal', () => {
      if (this.data.type === 'EDIT') {
        this.userForm.reset(this.data.userRegister);
      }
    });
  }

  // Verificar campo invalido.
  public inputIsInvalid(field: string) {
    return this.userForm.controls[field].errors && this.userForm.controls[field].touched;
  }

  // guardar usuario.
  public saveChanges(): void {
    if (this.userForm.invalid) {
      this.userForm.markAllAsTouched();
      return;
    }
    const userRegister: UserRegister = new UserRegister();
    userRegister.userName = this.userForm.get('userName')?.value;
    userRegister.email = this.userForm.get('email')?.value;
    userRegister.role = this.userForm.get('role')?.value;
    userRegister.password = '5ebe2294ecd0e0f08eab7690d2a6ee69';
    // Guardar datos, sólo si es válido el formulario.
    if (this.data.type === 'ADD') {
      this.userService.create(userRegister)
        .subscribe(result => this.responseData.emit(result));
    }
    if (this.data.type === 'EDIT') {
      this.userService.update(this.data.userId, userRegister)
        .subscribe(result => this.responseData.emit(result));
    }
  }

}
