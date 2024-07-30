import {Component, inject} from "@angular/core";
import {
  faAddressBook,
  faBox,
  faCoins,
  faFolderOpen,
  faScrewdriverWrench,
  faWallet
} from "@fortawesome/free-solid-svg-icons";
import {UserDataService} from "app/common/user-data.service";
import {AccountContainerComponent} from "app/common/containers/account-container/account-container.component";
import {NgClass, NgForOf, NgIf} from "@angular/common";
import {RouterLink} from "@angular/router";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";

@Component({
  selector: "app-shortcuts",
  standalone: true,
  imports: [
    AccountContainerComponent,
    NgForOf,
    NgClass,
    RouterLink,
    FaIconComponent,
    NgIf
  ],
  templateUrl: "./shortcuts.component.html"
})
export class ShortcutsComponent {
  private userDataService: UserDataService = inject(UserDataService);
  faAddressBook = faAddressBook;
  faBox = faBox;
  faFolderOpen = faFolderOpen;
  faCoins = faCoins;
  faScrewdriverWrench = faScrewdriverWrench;
  faWallet = faWallet;

}
