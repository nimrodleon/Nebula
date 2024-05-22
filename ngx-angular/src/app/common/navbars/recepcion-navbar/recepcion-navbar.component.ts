import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { DropdownMenuComponent } from '../dropdown-menu/dropdown-menu.component';
import { AuthNavbarNavComponent } from '../auth-navbar-nav/auth-navbar-nav.component';
import { faCheckToSlot } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-recepcion-navbar',
  standalone: true,
  imports: [
    RouterLink,
    FaIconComponent,
    DropdownMenuComponent,
    AuthNavbarNavComponent,
  ],
  templateUrl: './recepcion-navbar.component.html',
})
export class RecepcionNavbarComponent {
  @Input()
  companyId: string = "";
  faCheckToSlot = faCheckToSlot;
}
