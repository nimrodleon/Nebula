import {Component, Input} from "@angular/core";
import {faWallet} from "@fortawesome/free-solid-svg-icons";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {RouterModule} from "@angular/router";
import {DropdownMenuComponent} from "../dropdown-menu/dropdown-menu.component";
import {AuthNavbarNavComponent} from "../auth-navbar-nav/auth-navbar-nav.component";

@Component({
  selector: "app-cashier-navbar",
  standalone: true,
  imports: [
    FaIconComponent,
    RouterModule,
    DropdownMenuComponent,
    AuthNavbarNavComponent,
  ],
  templateUrl: "./cashier-navbar.component.html"
})
export class CashierNavbarComponent {
  faWallet = faWallet;
  @Input()
  companyId: string = "";
}
