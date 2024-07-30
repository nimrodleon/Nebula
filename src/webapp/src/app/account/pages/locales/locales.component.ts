import {Component} from "@angular/core";
import {AccountContainerComponent} from "app/common/containers/account-container/account-container.component";
import {faCirclePlus, faEye, faFolder, faPenToSquare} from "@fortawesome/free-solid-svg-icons";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {NgForOf} from "@angular/common";
import {RouterLink} from "@angular/router";

@Component({
  selector: "app-locales",
  standalone: true,
  imports: [
    AccountContainerComponent,
    FaIconComponent,
    NgForOf,
    RouterLink
  ],
  templateUrl: "./locales.component.html"
})
export class LocalesComponent {
  protected readonly faEye = faEye;
  protected readonly faPenToSquare = faPenToSquare;
  protected readonly faCirclePlus = faCirclePlus;
  protected readonly faFolder = faFolder;
}
