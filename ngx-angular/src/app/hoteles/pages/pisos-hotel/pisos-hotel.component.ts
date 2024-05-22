import { NgFor } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormControl, ReactiveFormsModule } from '@angular/forms';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faEdit, faFilter, faPlus, faSearch, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { CompanyDetailContainerComponent } from 'app/common/containers/company-detail-container/company-detail-container.component';
import { FormType, accessDenied, confirmTask, deleteConfirm } from 'app/common/interfaces';
import { UserDataService } from 'app/common/user-data.service';
import { PisoHotelModalComponent } from 'app/hoteles/components/piso-hotel-modal/piso-hotel-modal.component';
import { PisoHotel, PisoHotelDataModal } from 'app/hoteles/interfaces';
import { PisoHotelService } from 'app/hoteles/services';
import _ from "lodash";

declare const bootstrap: any;

@Component({
  selector: 'app-pisos-hotel',
  standalone: true,
  imports: [
    NgFor,
    FaIconComponent,
    CompanyDetailContainerComponent,
    PisoHotelModalComponent,
    ReactiveFormsModule
  ],
  templateUrl: './pisos-hotel.component.html'
})
export class PisosHotelComponent implements OnInit {
  private fb: FormBuilder = inject(FormBuilder);
  private pisoHotelService: PisoHotelService = inject(PisoHotelService);
  private userDataService: UserDataService = inject(UserDataService);
  faPlus = faPlus;
  faFilter = faFilter;
  faSearch = faSearch;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  pisoHotelModal: any;
  pisoHotelDataModal: PisoHotelDataModal = new PisoHotelDataModal();
  pisosHotel: Array<PisoHotel> = new Array<PisoHotel>();
  query: FormControl = this.fb.control("");

  ngOnInit(): void {
    this.pisoHotelModal = new bootstrap.Modal("#pisoHotelModal");
    this.cargarPisos();
  }

  public cargarPisos(): void {
    this.pisoHotelService.index(this.query.value)
      .subscribe(result => this.pisosHotel = result);
  }

  public submitSearch(e: Event): void {
    e.preventDefault();
    this.cargarPisos();
  }

  public agregarPisoHotelModal(): void {
    this.pisoHotelDataModal.type = FormType.ADD;
    this.pisoHotelDataModal.title = "Agregar Piso";
    this.pisoHotelDataModal.pisoHotel = new PisoHotel();
    this.pisoHotelModal.show();
  }

  public editarPisoHotelModal(item: PisoHotel): void {
    this.pisoHotelDataModal.type = FormType.EDIT;
    this.pisoHotelDataModal.title = "Editar Piso";
    this.pisoHotelDataModal.pisoHotel = item;
    this.pisoHotelModal.show();
  }

  public responseModal(data: PisoHotelDataModal): void {
    if (data.type === FormType.ADD) {
      this.pisosHotel = _.concat(data.pisoHotel, this.pisosHotel);
      this.pisoHotelModal.hide();
    }
    if (data.type === FormType.EDIT) {
      this.pisosHotel = _.map(this.pisosHotel, item => {
        if (item.id === data.pisoHotel.id) item = data.pisoHotel;
        return item;
      });
      this.pisoHotelModal.hide();
    }
  }

  public deletePisoItem(id: string): void {
    if (!this.userDataService.canDelete()) {
      accessDenied().then(() => console.log("accessDenied"));
    } else {
      deleteConfirm().then(result => {
        if (result.isConfirmed) {
          this.pisoHotelService.delete(id)
            .subscribe(result => {
              this.pisosHotel = _.filter(this.pisosHotel, item => item.id !== result.id);
            });
        }
      });
    }
  }

}
