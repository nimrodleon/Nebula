import {Component, OnInit} from '@angular/core';
import {
  faAddressBook, faArchive, faBox, faCashRegister, faCogs, faShoppingBasket, faWallet,
} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
  faBox = faBox;
  faAddressBook = faAddressBook;
  faCashRegister = faCashRegister;
  faShoppingBasket = faShoppingBasket;
  faCogs = faCogs;
  faWallet = faWallet;
  faArchive = faArchive;

  constructor() {
  }

  ngOnInit(): void {
  }

}
