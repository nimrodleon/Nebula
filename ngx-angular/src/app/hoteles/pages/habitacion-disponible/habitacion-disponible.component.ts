import { CurrencyPipe, NgClass, NgFor } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormControl, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faMagnifyingGlass, faRightToBracket } from '@fortawesome/free-solid-svg-icons';
import { RecepcionContainerComponent } from 'app/common/containers/recepcion-container/recepcion-container.component';
import { PaginationResult } from 'app/common/interfaces';
import { SplitPipe } from 'app/common/pipes/split.pipe';
import { EstadosRecepcionComponent } from 'app/hoteles/components/estados-recepcion/estados-recepcion.component';
import { HabitacionDisponible } from 'app/hoteles/interfaces';
import { RecepcionService } from 'app/hoteles/services';

@Component({
  selector: 'app-habitacion-disponible',
  standalone: true,
  imports: [
    NgFor,
    SplitPipe,
    NgClass,
    RouterLink,
    CurrencyPipe,
    FaIconComponent,
    ReactiveFormsModule,
    RecepcionContainerComponent,
    EstadosRecepcionComponent
  ],
  templateUrl: './habitacion-disponible.component.html'
})
export class HabitacionDisponibleComponent implements OnInit {
  private fb: FormBuilder = inject(FormBuilder);
  private route: ActivatedRoute = inject(ActivatedRoute);
  private recepcionService: RecepcionService = inject(RecepcionService);
  query: FormControl = this.fb.control("");
  habitacionesDisponibles: PaginationResult<HabitacionDisponible> = new PaginationResult<HabitacionDisponible>();
  faMagnifyingGlass = faMagnifyingGlass;
  faRightToBracket = faRightToBracket;
  companyId: string = "";

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
    });
    this.route.queryParams.subscribe(params => {
      const page = params["page"];
      this.cargarHabitacionesDisponibles(page);
    });
  }

  public submitSearch(e: any): void {
    e.preventDefault();
    this.cargarHabitacionesDisponibles();
  }

  private cargarHabitacionesDisponibles(page: number = 1): void {
    this.recepcionService.getHabitacionesDisponibles(this.query.value, page)
      .subscribe(result => this.habitacionesDisponibles = result);
  }



}
