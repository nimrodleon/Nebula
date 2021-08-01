import {Component, OnInit} from '@angular/core';
import {
  faBox,
  faCashRegister,
  faCog,
  faShoppingBasket,
  faUserAlt,
  faWarehouse
} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  faBox = faBox;
  faShoppingBasket = faShoppingBasket;
  faCashRegister = faCashRegister;
  faUserAlt = faUserAlt;
  // ============================================================
  faCog = faCog;
  faWarehouse = faWarehouse;

  constructor() {
  }

  ngOnInit(): void {
  }

}
