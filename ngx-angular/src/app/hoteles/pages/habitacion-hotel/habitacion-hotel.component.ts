import { CurrencyPipe, NgFor } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormControl, ReactiveFormsModule } from '@angular/forms';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faEdit, faFilter, faPlus, faSearch, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { CompanyDetailContainerComponent } from 'app/common/containers/company-detail-container/company-detail-container.component';
import { FormType, accessDenied, deleteConfirm } from 'app/common/interfaces';
import { UserDataService } from 'app/common/user-data.service';
import { HabitacionHotelModalComponent } from 'app/hoteles/components/habitacion-hotel-modal/habitacion-hotel-modal.component';
import { HabitacionHotel, HabitacionHotelDataModal } from 'app/hoteles/interfaces';
import { HabitacionHotelService } from 'app/hoteles/services';
import _ from 'lodash';

declare const bootstrap: any;

@Component({
  selector: 'app-habitacion-hotel',
  standalone: true,
  imports: [
    NgFor,
    CurrencyPipe,
    ReactiveFormsModule,
    FaIconComponent,
    CompanyDetailContainerComponent,
    HabitacionHotelModalComponent
  ],
  templateUrl: './habitacion-hotel.component.html'
})
export class HabitacionHotelComponent implements OnInit {
  private fb: FormBuilder = inject(FormBuilder);
  private habitacionHotelService: HabitacionHotelService = inject(HabitacionHotelService);
  private userDataService: UserDataService = inject(UserDataService);
  faPlus = faPlus;
  faFilter = faFilter;
  faSearch = faSearch;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  habitacionHotelModal: any;
  habitacionHotelDataModal: HabitacionHotelDataModal = new HabitacionHotelDataModal();
  habitacionesHotel: Array<HabitacionHotel> = new Array<HabitacionHotel>();
  query: FormControl = this.fb.control("");

  ngOnInit(): void {
    this.habitacionHotelModal = new bootstrap.Modal("#habitacionHotelModal");
    this.cargarHabitacionesHotel();
  }

  public cargarHabitacionesHotel(): void {
    this.habitacionHotelService.index(this.query.value)
      .subscribe(result => this.habitacionesHotel = result);
  }

  public submitSearch(e: Event): void {
    e.preventDefault();
    this.cargarHabitacionesHotel();
  }

  public agregarHabitacionHotelModal(): void {
    this.habitacionHotelDataModal.type = FormType.ADD;
    this.habitacionHotelDataModal.title = "Agregar Habitación";
    this.habitacionHotelDataModal.habitacionHotel = new HabitacionHotel();
    this.habitacionHotelModal.show();
  }

  public editarHabitacionHotelModal(item: HabitacionHotel): void {
    this.habitacionHotelDataModal.type = FormType.EDIT;
    this.habitacionHotelDataModal.title = "Editar Habitación";
    this.habitacionHotelDataModal.habitacionHotel = item;
    this.habitacionHotelModal.show();
  }

  public responseModal(data: HabitacionHotelDataModal): void {
    if (data.type === FormType.ADD) {
      this.habitacionesHotel = _.concat(data.habitacionHotel, this.habitacionesHotel);
      this.habitacionHotelModal.hide();
    }
    if (data.type === FormType.EDIT) {
      this.habitacionesHotel = _.map(this.habitacionesHotel, item => {
        if (item.id === data.habitacionHotel.id) item = data.habitacionHotel;
        return item;
      });
      this.habitacionHotelModal.hide();
    }
  }

  public deleteHabitacionHotelItem(id: string): void {
    if (!this.userDataService.canDelete()) {
      accessDenied().then(() => console.log("accessDenied"));
    } else {
      deleteConfirm().then(result => {
        if (result.isConfirmed) {
          this.habitacionHotelService.delete(id)
            .subscribe(result => {
              this.habitacionesHotel = _.filter(this.habitacionesHotel, item => item.id !== result.id);
            });
        }
      });
    }
  }

}
