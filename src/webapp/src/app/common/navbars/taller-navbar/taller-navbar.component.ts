import {Component} from "@angular/core";
import {faScrewdriverWrench} from "@fortawesome/free-solid-svg-icons";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {DropdownMenuComponent} from "../dropdown-menu/dropdown-menu.component";
import {AuthNavbarNavComponent} from "../auth-navbar-nav/auth-navbar-nav.component";
import {RouterModule} from "@angular/router";

@Component({
  selector: "app-taller-navbar",
  standalone: true,
  imports: [
    RouterModule,
    FaIconComponent,
    DropdownMenuComponent,
    AuthNavbarNavComponent
  ],
  templateUrl: "./taller-navbar.component.html"
})
export class TallerNavbarComponent {
  faScrewdriverWrench = faScrewdriverWrench;
}
