import {Component} from "@angular/core";
import {faCoins} from "@fortawesome/free-solid-svg-icons";
import {DropdownMenuComponent} from "../dropdown-menu/dropdown-menu.component";
import {AuthNavbarNavComponent} from "../auth-navbar-nav/auth-navbar-nav.component";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {RouterModule} from "@angular/router";

@Component({
  selector: "app-receivable-navbar",
  standalone: true,
  imports: [
    RouterModule,
    DropdownMenuComponent,
    AuthNavbarNavComponent,
    FaIconComponent
  ],
  templateUrl: "./receivable-navbar.component.html"
})
export class ReceivableNavbarComponent {
  faCoins = faCoins;
}
