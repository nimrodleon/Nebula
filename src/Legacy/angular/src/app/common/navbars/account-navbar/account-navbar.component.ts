import {Component} from "@angular/core";
import {faUserCircle} from "@fortawesome/free-solid-svg-icons";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {RouterLink} from "@angular/router";
import {AuthNavbarNavComponent} from "../auth-navbar-nav/auth-navbar-nav.component";

@Component({
  selector: "app-account-navbar",
  standalone: true,
  imports: [
    FaIconComponent,
    RouterLink,
    AuthNavbarNavComponent
  ],
  templateUrl: "./account-navbar.component.html"
})
export class AccountNavbarComponent {
  faUserCircle = faUserCircle;
}
