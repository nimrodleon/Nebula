import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {faBars, faBox, faCog, faSignOutAlt, faThList, faUserCircle} from '@fortawesome/free-solid-svg-icons';
import {AuthService} from '../../user/services';
import {ConfigurationService} from '../../system/services';
import {AuthUser} from '../../user/interfaces';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  faUserCircle = faUserCircle;
  faSignOutAlt = faSignOutAlt;
  faCog = faCog;
  faThList = faThList;
  faBars = faBars;
  faBox = faBox;
  companyName: string = '';
  authUser: AuthUser = new AuthUser();

  constructor(
    private router: Router,
    private authService: AuthService,
    private companyService: ConfigurationService) {
  }

  ngOnInit(): void {
    // cargar nombre empresa.
    this.companyService.show()
      .subscribe(result => this.companyName = result.rznSocial);
    // cargar usuario autentificado.
    this.authService.getMe().subscribe(result => this.authUser = result);
  }

  public logout(): void {
    this.authService.logout();
  }

}
