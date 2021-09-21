import {Component, OnInit} from '@angular/core';
import {
  faAddressBook, faArrowLeft,
  faArrowRight,
  faBox, faCashRegister, faCogs,
  faPiggyBank, faShoppingBasket, faWallet
} from '@fortawesome/free-solid-svg-icons';
import {Router} from '@angular/router';

enum Enum {
  true = '4365290483627856',
  false = '0363176613952597',
}

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
  faBox = faBox;
  faArrowRight = faArrowRight;
  faArrowLeft = faArrowLeft;
  faAddressBook = faAddressBook;
  faCashRegister = faCashRegister;
  faShoppingBasket = faShoppingBasket;
  faPiggyBank = faPiggyBank;
  faCogs = faCogs;
  faWallet = faWallet;

  // ====================================================================================================

  mnRoot: string | null = Enum.true;
  mnChildShopping: string | null = Enum.false;
  mnChildSales: string | null = Enum.false;

  constructor(
    private router: Router) {
  }

  ngOnInit(): void {
    // class por defecto mainMenu.
    const mainMenu: any = document.getElementById('mainMenu');
    mainMenu.classList.value = localStorage.getItem('classMainMenu');
    // valores por defecto.
    this.mnRoot = localStorage.getItem('mnRoot');
    this.mnChildShopping = localStorage.getItem('mnChildShopping');
    this.mnChildSales = localStorage.getItem('mnChildSales');
  }

  // Volver atrás.
  mnBack(e: any): void {
    e.preventDefault();
    this.mnRoot = Enum.true;
    localStorage.setItem('mnRoot', this.mnRoot);
    // menu compras.
    this.mnChildShopping = Enum.false;
    localStorage.setItem('mnChildShopping', this.mnChildShopping);
    // menu ventas.
    this.mnChildSales = Enum.false;
    localStorage.setItem('mnChildSales', this.mnChildSales);
  }

  // menu compras.
  async mnShopping(e: any) {
    e.preventDefault();
    this.mnRoot = Enum.false;
    this.mnChildShopping = Enum.true;
    localStorage.setItem('mnRoot', this.mnRoot);
    localStorage.setItem('mnChildShopping', this.mnChildShopping);
    await this.router.navigate(['/shopping']);
  }

  // menu ventas.
  async mnSales(e: any) {
    e.preventDefault();
    this.mnRoot = Enum.false;
    this.mnChildSales = Enum.true;
    localStorage.setItem('mnRoot', this.mnRoot);
    localStorage.setItem('mnChildSales', this.mnChildSales);
    await this.router.navigate(['/sales']);
  }

  // ====================================================================================================

  // verificar menu principal.
  checkMenuRoot(): boolean {
    return this.mnRoot === Enum.true;
  }

  // verificar submenu compras.
  checkMenuChildCompras(): boolean {
    return this.mnChildShopping === Enum.true;
  }

  // verificar submenu ventas.
  checkMenuChildSales(): boolean {
    return this.mnChildSales == Enum.true;
  }

}
