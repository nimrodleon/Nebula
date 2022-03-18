import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {faBars} from '@fortawesome/free-solid-svg-icons';
import {User, UserDataModal} from '../../interfaces';
import {ResponseData} from 'src/app/global/interfaces';
import {UserService} from '../../services';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {
  faBars = faBars;
  @Input() data: UserDataModal = new UserDataModal();
  @Output() responseData = new EventEmitter<ResponseData<User>>();
  userForm: FormGroup = this.fb.group({
    password: ['', [Validators.required]],
  });

  constructor(
    private fb: FormBuilder,
    private userService: UserService) {
  }

  ngOnInit(): void {
    const myModal: any = document.querySelector('#change-password');
    myModal.addEventListener('hide.bs.modal', () => {
      if (this.data.type === 'EDIT') {
        this.userForm.reset();
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
    // Guardar datos, sólo si es válido el formulario.
    if (this.data.type === 'EDIT') {
      this.userService.passwordChange(this.data.userId, this.userForm.get('password')?.value)
        .subscribe(result => this.responseData.emit(result));
    }
  }

}
