import { Component, inject } from "@angular/core";
import {
  faAddressBook,
  faBasketShopping,
  faBox, faCartFlatbed,
  faCog,
  faCoins,
  faFolderOpen,
  faScrewdriverWrench,
  faWallet
} from "@fortawesome/free-solid-svg-icons";
import { UserDataService } from "app/common/user-data.service";
import { AccountContainerComponent } from "app/common/containers/account-container/account-container.component";
import { NgClass, NgForOf } from "@angular/common";
import { RouterLink } from "@angular/router";
import { FaIconComponent } from "@fortawesome/angular-fontawesome";

@Component({
  selector: "app-shortcuts",
  standalone: true,
  imports: [
    AccountContainerComponent,
    NgForOf,
    NgClass,
    RouterLink,
    FaIconComponent
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
  faCartFlatbed = faCartFlatbed;
  faBasketShopping = faBasketShopping;
  faCog = faCog;

  public get companies() {
    return this.userDataService.companies;
  }

  public get isUserTypeAdmin(): boolean {
    return this.userDataService.isUserTypeAdmin();
  }

  public get isUserTypeDistributor(): boolean {
    return this.userDataService.isUserTypeDistributor();
  }

  public get isUserTypeCustomer(): boolean {
    return this.userDataService.isUserTypeCustomer();
  }

}
