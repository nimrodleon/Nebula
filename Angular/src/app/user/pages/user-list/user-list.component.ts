import {Component, OnInit} from '@angular/core';
import {faEdit, faFilter, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';

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
  title: string = '';
  userModal: any;
  changePasswordModal: any;

  constructor() {
  }

  ngOnInit(): void {
    // modal usuario.
    this.userModal = new bootstrap.Modal(document.querySelector('#user-modal'));
    // modal cambiar contraseña.
    this.changePasswordModal = new bootstrap.Modal(document.querySelector('#change-password'));
  }

  // abrir modal usuario.
  public showUserModal(): void {
    this.title = 'Agregar Usuario';
    this.userModal.show();
  }

  // abrir modal cambiar contraseña.
  public showChangePassword(): void {
    this.changePasswordModal.show();
  }

}
