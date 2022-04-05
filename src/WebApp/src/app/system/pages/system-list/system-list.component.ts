import {Component, OnInit} from '@angular/core';
import {faCog, faReceipt, faTags, faUserCog, faWarehouse} from '@fortawesome/free-solid-svg-icons';
import {AuthUser} from 'src/app/user/interfaces';
import {AuthService} from 'src/app/user/services';

@Component({
  selector: 'app-system-list',
  templateUrl: './system-list.component.html'
})
export class SystemListComponent implements OnInit {
  faCog = faCog;
  faUserCog = faUserCog;
  faWarehouse = faWarehouse;
  faReceipt = faReceipt;
  faTags = faTags;
  authUser: AuthUser = new AuthUser();

  constructor(
    private authService: AuthService) {
  }

  ngOnInit(): void {
    // cargar usuario autentificado.
    this.authService.getMe().subscribe(result => this.authUser = result);
  }

}
