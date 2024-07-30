import {Component, inject} from "@angular/core";
import {faBriefcase, faFolderOpen, faShareFromSquare, faUsersCog} from "@fortawesome/free-solid-svg-icons";
import {AccountNavbarComponent} from "../../navbars/account-navbar/account-navbar.component";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {RouterLink, RouterLinkActive} from "@angular/router";
import {UserDataService} from "app/common/user-data.service";

@Component({
  selector: "app-account-container",
  standalone: true,
  imports: [
    AccountNavbarComponent,
    FaIconComponent,
    RouterLink,
    RouterLinkActive,
  ],
  templateUrl: "./account-container.component.html"
})
export class AccountContainerComponent {
  private userDataService: UserDataService = inject(UserDataService);
  faShareFromSquare = faShareFromSquare;
  faFolderOpen = faFolderOpen;
  fafaUsersCog = faUsersCog;

  public get isBusinessType(): boolean {
    return this.userDataService.isUserAdmin()
      && this.userDataService.isBusinessType();
  }

  protected readonly faBriefcase = faBriefcase;
}
