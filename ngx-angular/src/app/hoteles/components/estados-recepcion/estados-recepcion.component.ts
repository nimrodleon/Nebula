import { Component, Input } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faCircleRight, faTrashAlt, faTriangleExclamation } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-estados-recepcion',
  standalone: true,
  imports: [
    RouterLink,
    RouterLinkActive,
    FaIconComponent,
  ],
  templateUrl: './estados-recepcion.component.html'
})
export class EstadosRecepcionComponent {
  @Input()
  companyId: string = "";
  faCircleRight = faCircleRight;
  faTriangleExclamation = faTriangleExclamation;
  faTrashAlt = faTrashAlt;
}
