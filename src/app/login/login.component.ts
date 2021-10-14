import {Component, OnInit} from '@angular/core';
import {faLock, faSignInAlt, faUser} from '@fortawesome/free-solid-svg-icons';

enum Enum {
  true = '4365290483627856',
  false = '0363176613952597',
}

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
    localStorage.setItem('mnRoot', Enum.true);
    localStorage.setItem('mnChildInventory', Enum.false);
    localStorage.setItem('mnChildShopping', Enum.false);
    localStorage.setItem('mnChildSales', Enum.false);
    localStorage.setItem('mnChildConfig', Enum.false);
  }

}
