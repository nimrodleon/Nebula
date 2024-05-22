import { Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faMagnifyingGlass, faRetweet } from '@fortawesome/free-solid-svg-icons';
import { RecepcionContainerComponent } from 'app/common/containers/recepcion-container/recepcion-container.component';
import { CambiarEstadoHabitacionComponent } from 'app/hoteles/components/cambiar-estado-habitacion/cambiar-estado-habitacion.component';
import { EstadosRecepcionComponent } from 'app/hoteles/components/estados-recepcion/estados-recepcion.component';

declare const bootstrap: any;

@Component({
  selector: 'app-habitacion-limpieza',
  standalone: true,
  imports: [
    FaIconComponent,
    EstadosRecepcionComponent,
    RecepcionContainerComponent,
    CambiarEstadoHabitacionComponent,
  ],
  templateUrl: './habitacion-limpieza.component.html'
})
export class HabitacionLimpiezaComponent {
  private route: ActivatedRoute = inject(ActivatedRoute);
  faMagnifyingGlass = faMagnifyingGlass;
  faRetweet = faRetweet;
  companyId: string = "";
  cambiarEstadoHabitacionModal: any;

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
    });
    this.cambiarEstadoHabitacionModal = new bootstrap.Modal("#cambiarEstadoHabitacionModal");
  }

  public cambiarEstadoModal(): void {
    this.cambiarEstadoHabitacionModal.show();
  }

}
