import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl} from '@angular/forms';
import {faEdit, faFilter, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {ResponseData} from 'src/app/global/interfaces';
import {User, UserDataModal} from '../../interfaces';
import {UserService} from '../../services';

declare var bootstrap: any;

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {
  faFilter = faFilter;
  faSearch = faSearch;
  faPlus = faPlus;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  userDataModal: UserDataModal = new UserDataModal();
  userList: Array<User> = new Array<User>();
  query: FormControl = this.fb.control('');
  userModal: any;
  changePasswordModal: any;

  constructor(
    private fb: FormBuilder,
    private userService: UserService) {
  }

  ngOnInit(): void {
    // modal usuario.
    this.userModal = new bootstrap.Modal(document.querySelector('#user-modal'));
    // modal cambiar contraseña.
    this.changePasswordModal = new bootstrap.Modal(document.querySelector('#change-password'));
    // cargar lista de usuarios.
    this.getUsers();
  }

  // cargar lista de usuarios.
  private getUsers(): void {
    this.userService.index(this.query.value)
      .subscribe(result => this.userList = result);
  }

  // formulario buscar usuarios.
  public submitSearch(e: Event): void {
    e.preventDefault();
    this.getUsers();
  }

  // abrir modal usuario.
  public showUserModal(): void {
    this.userDataModal.title = 'Agregar Usuario';
    this.userDataModal.type = 'ADD';
    this.userModal.show();
  }

  // abrir modal cambiar contraseña.
  public showChangePassword(id: any): void {
    this.userDataModal.title = 'Cambiar Contraseña';
    this.userDataModal.type = 'EDIT';
    this.userService.show(id).subscribe(result => {
      this.userDataModal.userId = result.id;
      this.changePasswordModal.show();
    });
  }

  // abrir modal usuario modo edición.
  public editUserModal(id: any): void {
    this.userDataModal.type = 'EDIT';
    this.userDataModal.title = 'Editar Usuario';
    this.userService.show(id).subscribe(result => {
      this.userDataModal.userId = result.id;
      this.userDataModal.userRegister.userName = result.userName;
      this.userDataModal.userRegister.email = result.email;
      this.userDataModal.userRegister.role = result.role;
      this.userModal.show();
    });
  }

  // ocultar modal usuario.
  public hideUserModal(response: ResponseData<User>): void {
    if (response.ok) {
      this.getUsers();
      this.userModal.hide();
      this.changePasswordModal.hide();
    }
  }

}
