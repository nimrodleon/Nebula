import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, ReactiveFormsModule } from '@angular/forms';
import { NgClass, NgFor, UpperCasePipe } from '@angular/common';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faEdit, faFilter, faPlus, faSearch, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { AccountContainerComponent } from 'app/common/containers/account-container/account-container.component';
import { UserModalComponent } from '../../components/user-modal/user-modal.component';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { PaginationResult } from 'app/common/interfaces';
import { User } from '../../interfaces';
import { UserService } from '../../services';

declare const bootstrap: any;

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [
    RouterLink,
    ReactiveFormsModule,
    FaIconComponent,
    AccountContainerComponent,
    UserModalComponent,
    NgFor,
    UpperCasePipe,
    NgClass,
  ],
  templateUrl: './user-list.component.html'
})
export class UserListComponent implements OnInit {
  faFilter = faFilter;
  faSearch = faSearch;
  faPlus = faPlus;
  faTrashAlt = faTrashAlt;
  faEdit = faEdit;
  userModal: any;
  users = new PaginationResult<User>();
  query: FormControl = this.fb.control("");

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private userService: UserService) { }

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      const page = params["page"];
      this.cargarUsuarios(page);
    });
    this.userModal = new bootstrap.Modal("#userModal");
  }

  private cargarUsuarios(page: number = 1): void {
    this.userService.index(this.query.value, page)
      .subscribe(result => this.users = result);
  }

  public addUserModal(): void {
    this.userModal.show();
  }

  public submitSearch(e: Event): void {
    e.preventDefault();
    this.cargarUsuarios();
  }

}
