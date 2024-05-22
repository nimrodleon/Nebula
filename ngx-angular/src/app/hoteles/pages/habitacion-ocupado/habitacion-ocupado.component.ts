import { NgFor } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faMagnifyingGlass, faRightFromBracket } from '@fortawesome/free-solid-svg-icons';
import { RecepcionContainerComponent } from 'app/common/containers/recepcion-container/recepcion-container.component';
import { EstadosRecepcionComponent } from 'app/hoteles/components/estados-recepcion/estados-recepcion.component';

@Component({
  selector: 'app-habitacion-ocupado',
  standalone: true,
  imports: [
    NgFor,
    RouterLink,
    FaIconComponent,
    EstadosRecepcionComponent,
    RecepcionContainerComponent
  ],
  templateUrl: './habitacion-ocupado.component.html'
})
export class HabitacionOcupadoComponent {
  private route: ActivatedRoute = inject(ActivatedRoute);
  faMagnifyingGlass = faMagnifyingGlass;
  faRightFromBracket = faRightFromBracket;
  companyId: string = "";

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
    });
  }

}
