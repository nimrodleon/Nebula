import {Component, OnInit} from '@angular/core';
import {faBox, faCashRegister, faShoppingBasket, faUserAlt} from '@fortawesome/free-solid-svg-icons';

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

  constructor() {
  }

  ngOnInit(): void {
  }

}
