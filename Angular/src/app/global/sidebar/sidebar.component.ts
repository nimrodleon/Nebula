import {Component, OnInit} from '@angular/core';
import {
  faAddressBook, faArchive, faArrowLeft,
  faArrowRight,
  faBox, faCashRegister, faCogs,
  faShoppingBasket, faWallet
} from '@fortawesome/free-solid-svg-icons';
import {Router} from '@angular/router';
import {EnumBoolean, EnumMenu} from '../interfaces';

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
  faCogs = faCogs;
  faWallet = faWallet;
  faArchive = faArchive;

  // ====================================================================================================

  rootMenu: string | null = EnumBoolean.true;
  mnChildInventory: string | null = EnumBoolean.false;
  mnChildShopping: string | null = EnumBoolean.false;
  mnChildSales: string | null = EnumBoolean.false;
  childMenuConfiguration: string | null = EnumBoolean.false;

  constructor(
    private router: Router) {
  }

  ngOnInit(): void {
    // class por defecto mainMenu.
    const mainMenu: any = document.getElementById('mainMenu');
    mainMenu.classList.value = localStorage.getItem('classMainMenu');
    // valores por defecto.
    this.rootMenu = localStorage.getItem(EnumMenu.rootMenu);
    this.mnChildInventory = localStorage.getItem(EnumMenu.childMenuInventory);
    this.mnChildShopping = localStorage.getItem(EnumMenu.childMenuShopping);
    this.mnChildSales = localStorage.getItem(EnumMenu.childMenuSales);
    this.childMenuConfiguration = localStorage.getItem(EnumMenu.childMenuConfiguration);
  }

  // Volver atrás.
  mnBack(e: Event): void {
    e.preventDefault();
    this.rootMenu = EnumBoolean.true;
    localStorage.setItem(EnumMenu.rootMenu, this.rootMenu);
    // menu inventario.
    this.mnChildInventory = EnumBoolean.false;
    localStorage.setItem(EnumMenu.childMenuInventory, this.mnChildInventory);
    // menu compras.
    this.mnChildShopping = EnumBoolean.false;
    localStorage.setItem(EnumMenu.childMenuShopping, this.mnChildShopping);
    // menu ventas.
    this.mnChildSales = EnumBoolean.false;
    localStorage.setItem(EnumMenu.childMenuSales, this.mnChildSales);
    // menu configuración.
    this.childMenuConfiguration = EnumBoolean.false;
    localStorage.setItem(EnumMenu.childMenuConfiguration, this.childMenuConfiguration);
  }

  // menu inventario.
  async mnInventory(e: Event, toggler: boolean = false) {
    e.preventDefault();
    this.rootMenu = EnumBoolean.false;
    this.mnChildInventory = EnumBoolean.true;
    localStorage.setItem(EnumMenu.rootMenu, this.rootMenu);
    localStorage.setItem(EnumMenu.childMenuInventory, this.mnChildInventory);
    if (!toggler) {
      await this.router.navigate(['/inventory']);
    }
  }

  // menu compras.
  async mnShopping(e: Event, toggler: boolean = false) {
    e.preventDefault();
    this.rootMenu = EnumBoolean.false;
    this.mnChildShopping = EnumBoolean.true;
    localStorage.setItem(EnumMenu.rootMenu, this.rootMenu);
    localStorage.setItem(EnumMenu.childMenuShopping, this.mnChildShopping);
    if (!toggler) {
      await this.router.navigate(['/shopping']);
    }
  }

  // menu ventas.
  async mnSales(e: Event, toggler: boolean = false) {
    e.preventDefault();
    this.rootMenu = EnumBoolean.false;
    this.mnChildSales = EnumBoolean.true;
    localStorage.setItem(EnumMenu.rootMenu, this.rootMenu);
    localStorage.setItem(EnumMenu.childMenuSales, this.mnChildSales);
    if (!toggler) {
      await this.router.navigate(['/sales']);
    }
  }

  // menu configuración.
  async mnConfig(e: Event, toggler: boolean = false) {
    e.preventDefault();
    this.rootMenu = EnumBoolean.false;
    this.childMenuConfiguration = EnumBoolean.true;
    localStorage.setItem(EnumMenu.rootMenu, this.rootMenu);
    localStorage.setItem(EnumMenu.childMenuConfiguration, this.childMenuConfiguration);
    if (!toggler) {
      await this.router.navigate(['/system']);
    }
  }

  // verificar menu principal.
  checkMenuRoot(): boolean {
    return this.rootMenu === EnumBoolean.true;
  }

  // verificar submenu inventario.
  checkMenuChildInventory(): boolean {
    return this.mnChildInventory === EnumBoolean.true;
  }

  // verificar submenu compras.
  checkMenuChildShopping(): boolean {
    return this.mnChildShopping === EnumBoolean.true;
  }

  // verificar submenu ventas.
  checkMenuChildSales(): boolean {
    return this.mnChildSales == EnumBoolean.true;
  }

  // verificar submenu ventas.
  checkMenuChildConfig(): boolean {
    return this.childMenuConfiguration == EnumBoolean.true;
  }

}
