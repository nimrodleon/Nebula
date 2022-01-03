import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl} from '@angular/forms';
import {faEdit, faFilter, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {UndMedida} from '../../interfaces';
import {UndMedidaService} from '../../services';
import {ResponseData} from '../../../global/interfaces';

declare var bootstrap: any;

@Component({
  selector: 'app-und-medida-list',
  templateUrl: './und-medida-list.component.html',
  styleUrls: ['./und-medida-list.component.scss']
})
export class UndMedidaListComponent implements OnInit {
  faFilter = faFilter;
  faSearch = faSearch;
  faPlus = faPlus;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  title: string = '';
  currentUndMedida: UndMedida = new UndMedida();
  unidadesDeMedida: Array<UndMedida> = new Array<UndMedida>();
  query: FormControl = this.fb.control('');
  undMedidaModal: any;

  constructor(
    private fb: FormBuilder,
    private undMedidaService: UndMedidaService) {
  }

  ngOnInit(): void {
    // modal unidad de medida.
    this.undMedidaModal = new bootstrap.Modal(document.querySelector('#und-medida-modal'));
    // cargar unidades de medida.
    this.getUnidadesDeMedida();
  }

  // cargar lista de unidades de medida.
  private getUnidadesDeMedida(): void {
    this.undMedidaService.index(this.query.value)
      .subscribe(result => this.unidadesDeMedida = result);
  }

  // buscar unidades de medida.
  public submitSearch(e: Event): void {
    e.preventDefault();
    this.getUnidadesDeMedida();
  }

  // abrir modal unidad de medida.
  public showUndMedidaModal(): void {
    this.title = 'Agregar Unidad Medida';
    this.currentUndMedida = new UndMedida();
    this.undMedidaModal.show();
  }

  // abrir modal und. medida modo edición.
  public editUndMedidaModal(id: any): void {
    this.title = 'Editar Unidad Medida';
    this.undMedidaService.show(id).subscribe(result => {
      this.currentUndMedida = result;
      this.undMedidaModal.show();
    });
  }

  // ocultar und. medida.
  public hideUndMedidaModal(response: ResponseData<UndMedida>): void {
    if (response.ok) {
      this.getUnidadesDeMedida();
      this.undMedidaModal.hide();
    }
  }

}
