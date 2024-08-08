import {Component, Input} from "@angular/core";
import {faBox} from "@fortawesome/free-solid-svg-icons";
import {DropdownMenuComponent} from "../dropdown-menu/dropdown-menu.component";
import {AuthNavbarNavComponent} from "../auth-navbar-nav/auth-navbar-nav.component";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {RouterModule} from "@angular/router";

@Component({
  selector: "app-product-navbar",
  standalone: true,
  imports: [
    RouterModule,
    DropdownMenuComponent,
    AuthNavbarNavComponent,
    FaIconComponent
  ],
  templateUrl: "./product-navbar.component.html"
})
export class ProductNavbarComponent {
  faBox = faBox;
  @Input()
  companyId: string = "";
}
