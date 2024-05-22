import { NgFor } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormControl, ReactiveFormsModule } from '@angular/forms';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faEdit, faFilter, faPlus, faSearch, faTrashAlt } from '@fortawesome/free-solid-svg-icons';
import { CompanyDetailContainerComponent } from 'app/common/containers/company-detail-container/company-detail-container.component';
import { FormType, accessDenied, deleteConfirm } from 'app/common/interfaces';
import { UserDataService } from 'app/common/user-data.service';
import { CategoriaHabitacionHotelComponent } from 'app/hoteles/components/categoria-habitacion-hotel/categoria-habitacion-hotel.component';
import { CategoriaHabitacion, CategoriaHabitacionDataModal } from 'app/hoteles/interfaces';
import { CategoriaHabitacionService } from 'app/hoteles/services';
import _ from 'lodash';

declare const bootstrap: any;

@Component({
  selector: 'app-categorias-habitacion',
  standalone: true,
  imports: [
    NgFor,
    FaIconComponent,
    CategoriaHabitacionHotelComponent,
    CompanyDetailContainerComponent,
    ReactiveFormsModule
  ],
  templateUrl: './categorias-habitacion.component.html',
})
export class CategoriasHabitacionComponent implements OnInit {
  private fb: FormBuilder = inject(FormBuilder);
  private categoriaHabitacionService: CategoriaHabitacionService = inject(CategoriaHabitacionService);
  private userDataService: UserDataService = inject(UserDataService);
  faPlus = faPlus;
  faFilter = faFilter;
  faSearch = faSearch;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  categoriaHabitacionModal: any;
  categoriaHabitacionDataModal: CategoriaHabitacionDataModal = new CategoriaHabitacionDataModal();
  categoriasHabitacion: Array<CategoriaHabitacion> = new Array<CategoriaHabitacion>();
  query: FormControl = this.fb.control("");

  ngOnInit(): void {
    this.categoriaHabitacionModal = new bootstrap.Modal("#categoriaHabitacionModal");
    this.cargarCategoriasHabitacion();
  }

  public cargarCategoriasHabitacion(): void {
    this.categoriaHabitacionService.index(this.query.value)
      .subscribe(result => this.categoriasHabitacion = result);
  }

  public submitSearch(e: Event): void {
    e.preventDefault();
    this.cargarCategoriasHabitacion();
  }

  public agregarCategoriaHabitacionModal(): void {
    this.categoriaHabitacionDataModal.type = FormType.ADD;
    this.categoriaHabitacionDataModal.title = "Agregar Categoría";
    this.categoriaHabitacionDataModal.categoriaHabitacion = new CategoriaHabitacion();
    this.categoriaHabitacionModal.show();
  }

  public editarCategoriaHabitacionModal(item: CategoriaHabitacion): void {
    this.categoriaHabitacionDataModal.type = FormType.EDIT;
    this.categoriaHabitacionDataModal.title = "Editar Categoría";
    this.categoriaHabitacionDataModal.categoriaHabitacion = item;
    this.categoriaHabitacionModal.show();
  }

  public responseModal(data: CategoriaHabitacionDataModal): void {
    if (data.type === FormType.ADD) {
      this.categoriasHabitacion = _.concat(data.categoriaHabitacion, this.categoriasHabitacion);
      this.categoriaHabitacionModal.hide();
    }
    if (data.type === FormType.EDIT) {
      this.categoriasHabitacion = _.map(this.categoriasHabitacion, item => {
        if (item.id === data.categoriaHabitacion.id) item = data.categoriaHabitacion;
        return item;
      });
      this.categoriaHabitacionModal.hide();
    }
  }

  public deleteCategoriaHabitacionItem(id: string): void {
    if (!this.userDataService.canDelete()) {
      accessDenied().then(() => console.log("accessDenied"));
    } else {
      deleteConfirm().then(result => {
        if (result.isConfirmed) {
          this.categoriaHabitacionService.delete(id)
            .subscribe(result => {
              this.categoriasHabitacion = _.filter(this.categoriasHabitacion, item => item.id !== result.id);
            });
        }
      });
    }
  }


}
