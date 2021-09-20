import {Component, OnInit} from '@angular/core';
import {
  faAddressBook,
  faArrowRight,
  faBox, faCashRegister, faCogs, faFileInvoice,
  faPiggyBank, faShoppingBasket, faWallet
} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
  faBox = faBox;
  faArrowRight = faArrowRight;
  faAddressBook = faAddressBook;
  faCashRegister = faCashRegister;
  faShoppingBasket = faShoppingBasket;
  faPiggyBank = faPiggyBank;
  faCogs = faCogs;
  faWallet = faWallet;

  constructor() {
  }

  ngOnInit(): void {
    // class por defecto mainMenu.
    const mainMenu: any = document.getElementById('mainMenu');
    mainMenu.classList.value = localStorage.getItem('classMainMenu');
  }

}
