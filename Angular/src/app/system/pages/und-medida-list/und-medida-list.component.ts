import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl} from '@angular/forms';
import {faEdit, faFilter, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';

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
  query: FormControl = this.fb.control('');
  undMedidaModal: any;

  constructor(private fb: FormBuilder) {
  }

  ngOnInit(): void {
    // modal unidad de medida.
    this.undMedidaModal = new bootstrap.Modal(document.querySelector('#und-medida-modal'));
  }

  // abrir modal unidad de medida.
  public showUndMedidaModal(): void {
    this.title = 'Agregar Unidad Medida';
    this.undMedidaModal.show();
  }

}
