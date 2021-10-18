import {Component, OnInit} from '@angular/core';
import {
  faBars,
  faBox, faCashRegister,
  faCog, faSignOutAlt,
  faUserCircle
} from '@fortawesome/free-solid-svg-icons';

declare var bootstrap: any;

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  faUserCircle = faUserCircle;
  faSignOutAlt = faSignOutAlt;
  faCog = faCog;
  faCashRegister = faCashRegister;
  faBars = faBars;
  faBox = faBox;
  // ========================================
  bsOffcanvas: any;

  constructor() {
  }

  ngOnInit(): void {
    // menu principal del punto de venta.
    if (document.getElementById('offcanvas')) {
      this.bsOffcanvas = new bootstrap.Offcanvas(document.getElementById('offcanvas'));
    }
  }

  // Toggle menu principal.
  mainMenuToggle(e: any) {
    e.preventDefault();
    const mainMenu: any = document.getElementById('mainMenu');
    if (mainMenu) {
      mainMenu.classList.toggle('hiddenNavigation');
      localStorage.setItem('classMainMenu', mainMenu.classList.value);
    }
    // mostrar menu principal del terminal.
    if (document.getElementById('offcanvas')) {
      this.bsOffcanvas.show();
    }
  }

}
