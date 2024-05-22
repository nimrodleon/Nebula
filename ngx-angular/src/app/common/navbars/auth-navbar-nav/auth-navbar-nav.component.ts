import {Component} from "@angular/core";
import {faCircleQuestion, faSignOutAlt, faUserCircle} from "@fortawesome/free-solid-svg-icons";
import {UserDataService} from "../../user-data.service";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {RouterLink} from "@angular/router";

@Component({
  selector: "app-auth-navbar-nav",
  standalone: true,
  imports: [
    FaIconComponent,
    RouterLink
  ],
  templateUrl: "./auth-navbar-nav.component.html"
})
export class AuthNavbarNavComponent {
  faCircleQuestion = faCircleQuestion;
  faUserCircle = faUserCircle;
  faSignOutAlt = faSignOutAlt;

  constructor(private userDataService: UserDataService) {
  }

  public get userAuth() {
    return this.userDataService.userAuth;
  }

  public logout(): void {
    this.userDataService.logout();
  }

  public abrirWhatsapp(e: Event): void {
    e.preventDefault();
    window.open("https://wa.me/51916533482", "_blank");
  }

}
