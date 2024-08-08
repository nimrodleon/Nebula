import {Component, Input} from "@angular/core";
import {faFolderOpen} from "@fortawesome/free-solid-svg-icons";
import {DropdownMenuComponent} from "../dropdown-menu/dropdown-menu.component";
import {AuthNavbarNavComponent} from "../auth-navbar-nav/auth-navbar-nav.component";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {RouterModule} from "@angular/router";

@Component({
  selector: "app-sales-navbar",
  standalone: true,
  imports: [
    RouterModule,
    DropdownMenuComponent,
    AuthNavbarNavComponent,
    FaIconComponent
  ],
  templateUrl: "./sales-navbar.component.html"
})
export class SalesNavbarComponent {
  faFolderOpen = faFolderOpen;
  @Input()
  companyId: string = "";
}
