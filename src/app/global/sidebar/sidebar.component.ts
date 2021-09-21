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
  mnChildCompras: string | null = Enum.false;

  constructor(
    private router: Router) {
  }

  ngOnInit(): void {
    // class por defecto mainMenu.
    const mainMenu: any = document.getElementById('mainMenu');
    mainMenu.classList.value = localStorage.getItem('classMainMenu');
    // valores por defecto.
    this.mnRoot = localStorage.getItem('mnRoot');
    this.mnChildCompras = localStorage.getItem('mnChildCompras');
  }

  // Volver atrás.
  mnBack(e: any): void {
    e.preventDefault();
    this.mnRoot = Enum.true;
    localStorage.setItem('mnRoot', this.mnRoot);
    // menu compras.
    this.mnChildCompras = Enum.false;
    localStorage.setItem('mnChildCompras', this.mnChildCompras);
  }

  // menu compras.
  async mnCompras(e: any) {
    e.preventDefault();
    this.mnRoot = Enum.false;
    this.mnChildCompras = Enum.true;
    localStorage.setItem('mnRoot', this.mnRoot);
    localStorage.setItem('mnChildCompras', this.mnChildCompras);
    await this.router.navigate(['/products']);
  }

  // ====================================================================================================

  // verificar menu principal.
  checkMenuRoot(): boolean {
    return this.mnRoot === Enum.true;
  }

  // verificar submenu compras.
  checkMenuChildCompras(): boolean {
    return this.mnChildCompras === Enum.true;
  }

}
