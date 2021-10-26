import {Component, OnInit} from '@angular/core';
import {
  faBars,
  faBox, faCashRegister,
  faCog, faSignOutAlt,
  faUserCircle
} from '@fortawesome/free-solid-svg-icons';
import {AuthService} from '../../user/services';

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
  mainMenu: any;

  constructor(private authService: AuthService) {
  }

  ngOnInit(): void {
    if (document.getElementById('mainMenu')) {
      this.mainMenu = document.getElementById('mainMenu');
    }
  }

  // Toggle menu principal.
  public mainMenuToggle(e: any) {
    e.preventDefault();
    if (this.mainMenu) {
      this.mainMenu.classList.toggle('hiddenNavigation');
      localStorage.setItem('classMainMenu', this.mainMenu.classList.value);
    }
  }

  public logout(): void {
    this.authService.logout();
  }

}
