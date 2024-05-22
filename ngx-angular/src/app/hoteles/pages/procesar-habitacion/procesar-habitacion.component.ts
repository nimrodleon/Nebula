import { CurrencyPipe } from '@angular/common';
import { Component, Injector, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faCircleLeft, faCoins, faPlus, faSave } from '@fortawesome/free-solid-svg-icons';
import { RecepcionContainerComponent } from 'app/common/containers/recepcion-container/recepcion-container.component';
import { initializeSelect2Injector, select2Contactos } from 'app/common/interfaces';
import { Contact } from 'app/contact/interfaces';
import { HabitacionHotel } from 'app/hoteles/interfaces';
import { HabitacionHotelService } from 'app/hoteles/services';

@Component({
  selector: 'app-procesar-habitacion',
  standalone: true,
  imports: [
    FormsModule,
    FaIconComponent,
    CurrencyPipe,
    RecepcionContainerComponent
  ],
  templateUrl: './procesar-habitacion.component.html'
})
export class ProcesarHabitacionComponent implements OnInit {
  private activatedRoute: ActivatedRoute = inject(ActivatedRoute);
  private habitacionHotelService: HabitacionHotelService = inject(HabitacionHotelService);
  habitacionHotel: HabitacionHotel = new HabitacionHotel();
  contacto: Contact = new Contact();
  faPlus = faPlus;
  faCoins = faCoins;
  faCircleLeft = faCircleLeft;
  faSave = faSave;

  constructor(private injector: Injector) {
    initializeSelect2Injector(injector);
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      const habitacionId = params.get("habitacionId") || "";
      this.habitacionHotelService.show(habitacionId).subscribe(result => this.habitacionHotel = result);
    });
    select2Contactos("#contacto").on("select2:select", (e: any) => {
      const data = e.params.data;
      this.contacto = { ...data };
    });
  }

}
