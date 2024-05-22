import { Component, inject } from '@angular/core';
import { UserDetailContainerComponent } from '../../components/user-detail-container/user-detail-container.component';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faFilter, faPlus, faSearch } from '@fortawesome/free-solid-svg-icons';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../../services';
import { Company } from 'app/account/interfaces';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-user-empresas',
  standalone: true,
  imports: [
    FaIconComponent,
    UserDetailContainerComponent,
    NgFor,
  ],
  templateUrl: './user-empresas.component.html'
})
export class UserEmpresasComponent {
  faFilter = faFilter;
  faSearch = faSearch;
  faPlus = faPlus;
  companies: Array<Company> = new Array<Company>();
  private activatedRoute: ActivatedRoute = inject(ActivatedRoute);
  private userService: UserService = inject(UserService);

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      const userId = params.get("userId") || "";
      this.userService.getCompanies(userId)
        .subscribe(result => this.companies = result);
    });
  }

}
