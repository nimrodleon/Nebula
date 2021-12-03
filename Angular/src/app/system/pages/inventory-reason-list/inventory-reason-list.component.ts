import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl} from '@angular/forms';
import {faEdit, faFilter, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';

declare var bootstrap: any;

@Component({
  selector: 'app-inventory-reason-list',
  templateUrl: './inventory-reason-list.component.html',
  styleUrls: ['./inventory-reason-list.component.scss']
})
export class InventoryReasonListComponent implements OnInit {
  faFilter = faFilter;
  faSearch = faSearch;
  faPlus = faPlus;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  title: string = '';
  query: FormControl = this.fb.control('');
  inventoryReasonModal: any;

  constructor(private fb: FormBuilder) {
  }

  ngOnInit(): void {
    // formulario motivos de inventario.
    this.inventoryReasonModal = new bootstrap.Modal(document.querySelector('#inventory-reason-modal'));
  }

  // abrir modal motivos de inventario.
  public showInventoryReasonModal(): void {
    this.title = 'Agregar Motivo';
    this.inventoryReasonModal.show();
  }


}
