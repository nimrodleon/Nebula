import {Component, Input} from "@angular/core";
import {faCartFlatbed} from "@fortawesome/free-solid-svg-icons";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {DropdownMenuComponent} from "../dropdown-menu/dropdown-menu.component";
import {AuthNavbarNavComponent} from "../auth-navbar-nav/auth-navbar-nav.component";
import {RouterModule} from "@angular/router";

@Component({
  selector: "app-inventory-navbar",
  standalone: true,
  imports: [
    RouterModule,
    FaIconComponent,
    DropdownMenuComponent,
    AuthNavbarNavComponent
  ],
  templateUrl: "./inventory-navbar.component.html"
})
export class InventoryNavbarComponent {
  faCartFlatbed = faCartFlatbed;
  @Input()
  companyId: string = "";
}
