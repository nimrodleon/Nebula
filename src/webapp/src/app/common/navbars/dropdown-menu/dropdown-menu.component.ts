import {Component, inject} from "@angular/core";
import {faShareNodes} from "@fortawesome/free-solid-svg-icons";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {RouterModule} from "@angular/router";
import {UserDataService} from "app/common/user-data.service";
import {NgIf} from "@angular/common";

@Component({
  selector: "app-dropdown-menu",
  standalone: true,
  imports: [
    RouterModule,
    FaIconComponent,
    NgIf,
  ],
  templateUrl: "./dropdown-menu.component.html"
})
export class DropdownMenuComponent {
  private userDataService: UserDataService = inject(UserDataService);
  faShareNodes = faShareNodes;
}
