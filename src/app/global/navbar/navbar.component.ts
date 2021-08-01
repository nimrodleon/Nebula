import {Component, OnInit} from '@angular/core';
import {
  faBox, faCashRegister,
  faCog, faServer,
  faShoppingBasket,
  faSignOutAlt,
  faThLarge,
  faUserCircle
} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  faServer = faServer;
  faUserCircle = faUserCircle;
  faSignOutAlt = faSignOutAlt;
  faThLarge = faThLarge;
  faCog = faCog;
  faCashRegister = faCashRegister;
  faShoppingBasket = faShoppingBasket;
  faBox = faBox;

  constructor() {
  }

  ngOnInit(): void {
  }

}
