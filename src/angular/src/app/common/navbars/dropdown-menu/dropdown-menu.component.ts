import {Component, Input} from "@angular/core";
import {faShareNodes} from "@fortawesome/free-solid-svg-icons";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {RouterModule} from "@angular/router";

@Component({
  selector: "app-dropdown-menu",
  standalone: true,
  imports: [
    RouterModule,
    FaIconComponent,
  ],
  templateUrl: "./dropdown-menu.component.html"
})
export class DropdownMenuComponent {
  faShareNodes = faShareNodes;
  @Input()
  companyId: string = "";
}
