import {Component} from "@angular/core";
import {faCertificate, faFolderOpen, faReceipt, faWarehouse} from "@fortawesome/free-solid-svg-icons";
import {ActivatedRoute, RouterLinkActive, RouterModule} from "@angular/router";
import {CompanyService} from "app/account/services";
import {LocalSummaryDto} from "app/account/interfaces";
import {AccountNavbarComponent} from "../../navbars/account-navbar/account-navbar.component";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";

@Component({
  selector: "app-company-detail-container",
  standalone: true,
  imports: [
    RouterModule,
    AccountNavbarComponent,
    FaIconComponent,
    RouterLinkActive
  ],
  templateUrl: "./company-detail-container.component.html"
})
export class CompanyDetailContainerComponent {
  faWarehouse = faWarehouse;
  faReceipt = faReceipt;
  faFolderOpen = faFolderOpen;
  faCertificate = faCertificate;
  companyId: string = "";
  companyInfo: LocalSummaryDto = new LocalSummaryDto();

  constructor(
    private route: ActivatedRoute,
    private companyService: CompanyService) {
    this.route.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
      if (this.companyId !== "") {
        this.companyService.getCompanyInfo(this.companyId)
          .subscribe(result => this.companyInfo = result);
      }
    });
  }

}
