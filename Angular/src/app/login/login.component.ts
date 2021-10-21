import {Component, OnInit} from '@angular/core';
import {faLock, faSignInAlt, faUser} from '@fortawesome/free-solid-svg-icons';
import {EnumBoolean, EnumMenu} from '../global/interfaces';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  faUser = faUser;
  faLock = faLock;
  faSignInAlt = faSignInAlt;

  constructor() {
  }

  ngOnInit(): void {
    // inicializar valores del menu principal.
    localStorage.setItem(EnumMenu.rootMenu, EnumBoolean.true);
    // localStorage.setItem(EnumMenu.childMenuInventory, EnumBoolean.false);
    // localStorage.setItem(EnumMenu.childMenuShopping, EnumBoolean.false);
    // localStorage.setItem(EnumMenu.childMenuSales, EnumBoolean.false);
    localStorage.setItem(EnumMenu.childMenuConfiguration, EnumBoolean.false);
  }

}
