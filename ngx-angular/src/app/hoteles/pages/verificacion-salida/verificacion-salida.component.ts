import { Component } from '@angular/core';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faCircleLeft, faCoins, faSave } from '@fortawesome/free-solid-svg-icons';
import { RecepcionContainerComponent } from 'app/common/containers/recepcion-container/recepcion-container.component';

@Component({
  selector: 'app-verificacion-salida',
  standalone: true,
  imports: [
    FaIconComponent,
    RecepcionContainerComponent
  ],
  templateUrl: './verificacion-salida.component.html'
})
export class VerificacionSalidaComponent {
  faCoins = faCoins;
  faCircleLeft = faCircleLeft;
  faSave = faSave;
}
