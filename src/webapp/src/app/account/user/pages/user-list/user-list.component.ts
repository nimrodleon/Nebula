import {Component, OnInit} from "@angular/core";
import {FormBuilder, FormControl, ReactiveFormsModule} from "@angular/forms";
import {NgClass, NgFor, NgIf, UpperCasePipe} from "@angular/common";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {faEdit, faFilter, faLock, faPlus, faSearch, faTrashAlt} from "@fortawesome/free-solid-svg-icons";
import {AccountContainerComponent} from "app/common/containers/account-container/account-container.component";
import {UserModalComponent} from "../../components/user-modal/user-modal.component";
import {ActivatedRoute, RouterLink} from "@angular/router";
import {deleteConfirm, FormType, PaginationResult} from "app/common/interfaces";
import {User, UserDataModal} from "../../interfaces";
import {UserPersonalService} from "../../services";
import {ChangePasswordModalComponent} from "../../components/change-password-modal/change-password-modal.component";
import _ from "lodash";

declare const bootstrap: any;

@Component({
  selector: "app-user-list",
  standalone: true,
  imports: [
    NgIf,
    RouterLink,
    ReactiveFormsModule,
    FaIconComponent,
    AccountContainerComponent,
    UserModalComponent,
    NgFor,
    UpperCasePipe,
    NgClass,
    ChangePasswordModalComponent,
  ],
  templateUrl: "./user-list.component.html"
})
export class UserListComponent implements OnInit {
  faFilter = faFilter;
  faSearch = faSearch;
  faPlus = faPlus;
  faTrashAlt = faTrashAlt;
  faEdit = faEdit;
  faLock = faLock;
  userModal: any;
  changePasswordModal: any;
  users = new PaginationResult<User>();
  query: FormControl = this.fb.control("");
  userDataModal: UserDataModal = new UserDataModal();

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private userPersonalService: UserPersonalService) {
  }

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      const page = params["page"];
      this.cargarUsuarios(page);
    });
    this.userModal = new bootstrap.Modal("#userModal");
    this.changePasswordModal = new bootstrap.Modal("#changePasswordModal");
  }

  private cargarUsuarios(page: number = 1): void {
    this.userPersonalService.index(this.query.value, page)
      .subscribe(result => this.users = result);
  }

  public addUserModal(): void {
    this.userDataModal.type = FormType.ADD;
    this.userDataModal.title = "Agregar Usuario";
    this.userDataModal.user = new User();
    this.userModal.show();
  }

  public changePasswordModalUser(user: User): void {
    this.userDataModal.type = FormType.EDIT;
    this.userDataModal.user = user;
    this.changePasswordModal.show();
  }

  public hideChangePasswordModal(result: boolean): void {
    if (result) this.changePasswordModal.hide();
  }

  public editarUserModal(user: User): void {
    this.userDataModal.type = FormType.EDIT;
    this.userDataModal.title = "Editar Usuario";
    this.userDataModal.user = user;
    this.userModal.show();
  }

  public submitSearch(e: Event): void {
    e.preventDefault();
    this.cargarUsuarios();
  }

  public responseModal(data: UserDataModal): void {
    if (data.type === FormType.ADD) {
      this.users.data = _.concat(data.user, this.users.data);
      this.userModal.hide();
    }
    if (data.type === FormType.EDIT) {
      this.users.data = _.map(this.users.data, item => {
        if (item.id === data.user.id) item = data.user;
        return item;
      });
      this.userModal.hide();
    }
  }

  public deleteUser(user: User): void {
    deleteConfirm().then(result => {
      if (result.isConfirmed) {
        this.userPersonalService.delete(user.id)
          .subscribe(() => {
            this.users.data = _.filter(this.users.data, item => item.id !== user.id);
          });
      }
    });
  }

}
