import {Component, OnInit} from "@angular/core";
import {faCirclePlus, faEye, faPenToSquare} from "@fortawesome/free-solid-svg-icons";
import {Company} from "../../interfaces";
import {CompanyService} from "../../services";
import {RouterLink} from "@angular/router";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {AccountContainerComponent} from "app/common/containers/account-container/account-container.component";
import {NgForOf} from "@angular/common";

@Component({
  selector: "app-company-list",
  standalone: true,
  imports: [
    RouterLink,
    FaIconComponent,
    AccountContainerComponent,
    NgForOf
  ],
  templateUrl: "./company-list.component.html"
})
export class CompanyListComponent implements OnInit {
  faPenToSquare = faPenToSquare;
  faEye = faEye;
  faCirclePlus = faCirclePlus;
  companies: Array<Company> = new Array<Company>();

  constructor(private companyService: CompanyService) {
  }

  ngOnInit(): void {
    this.companyService.getCompanies()
      .subscribe(result => this.companies = result);
  }

}
