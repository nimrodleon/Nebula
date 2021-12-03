import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl} from '@angular/forms';
import {faEdit, faFilter, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {Warehouse} from '../../interfaces';
import {WarehouseService} from '../../services';
import {ResponseData} from '../../../global/interfaces';

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
  currentWarehouse: Warehouse | any;
  warehouses: Array<Warehouse> = new Array<Warehouse>();
  query: FormControl = this.fb.control('');
  warehouseModal: any;

  constructor(
    private fb: FormBuilder,
    private warehouseService: WarehouseService) {
  }

  ngOnInit(): void {
    // formulario modal almacén.
    this.warehouseModal = new bootstrap.Modal(document.querySelector('#warehouse-modal'));
    // cargar lista de almacenes.
    this.getWarehouses();
  }

  // obtener lista de almacenes.
  private getWarehouses(): void {
    this.warehouseService.index(this.query.value).subscribe(result => this.warehouses = result);
  }

  // buscar almacén.
  public submitSearch(e: any): void {
    e.preventDefault();
    this.getWarehouses();
  }

  // abrir modal almacén.
  public showWarehouseModal(): void {
    this.title = 'Agregar Almacén';
    this.currentWarehouse = null;
    this.warehouseModal.show();
  }

  // abrir modal almacén modo edición.
  public editWarehouseModal(id: any): void {
    this.title = 'Editar Almacén';
    this.warehouseService.show(id).subscribe(result => {
      this.currentWarehouse = result;
      this.warehouseModal.show();
    });
  }

  // ocultar modal almacén.
  public hideWarehouseModal(response: ResponseData<Warehouse>): void {
    if (response.ok) {
      this.getWarehouses();
      this.warehouseModal.hide();
    }
  }

}
