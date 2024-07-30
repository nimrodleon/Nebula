import {Component} from "@angular/core";
import {AccountContainerComponent} from "app/common/containers/account-container/account-container.component";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {
  faCircleLeft,
  faCirclePlus,
  faClipboardList,
  faEdit, faSave,
  faTags,
  faTrashAlt
} from "@fortawesome/free-solid-svg-icons";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {NgForOf} from "@angular/common";
import {RouterLink} from "@angular/router";

@Component({
  selector: "app-local-form",
  standalone: true,
  imports: [
    AccountContainerComponent,
    FormsModule,
    ReactiveFormsModule,
    FaIconComponent,
    NgForOf,
    RouterLink
  ],
  templateUrl: "./local-form.component.html",
})
export class LocalFormComponent {

  protected readonly faClipboardList = faClipboardList;
  protected readonly faTags = faTags;
  protected readonly faEdit = faEdit;
  protected readonly faTrashAlt = faTrashAlt;
  protected readonly faCirclePlus = faCirclePlus;
  protected readonly faCircleLeft = faCircleLeft;
  protected readonly faSave = faSave;
}
