import {Component, OnInit} from '@angular/core';
import {faEdit, faFilter, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';

declare var bootstrap: any;

@Component({
  selector: 'app-warehouse-list',
  templateUrl: './warehouse-list.component.html',
  styleUrls: ['./warehouse-list.component.scss']
})
export class WarehouseListComponent implements OnInit {
  faFilter = faFilter;
  faSearch = faSearch;
  faPlus = faPlus;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  title: string = '';
  warehouseModal: any;

  constructor() {
  }

  ngOnInit(): void {
    // formulario modal almacén.
    this.warehouseModal = new bootstrap.Modal(document.querySelector('#warehouse-modal'));
  }

  // abrir modal almacén.
  public showWarehouseModal(): void {
    this.title = 'Agregar Almacén';
    this.warehouseModal.show();
  }

}
