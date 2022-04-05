import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl} from '@angular/forms';
import {faEdit, faFilter, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {AuthService} from 'src/app/user/services';
import {accessDenied, deleteConfirm, ResponseData} from 'src/app/global/interfaces';
import {WarehouseService} from '../../services';
import {Warehouse} from '../../interfaces';

declare var bootstrap: any;

@Component({
  selector: 'app-warehouse-list',
  templateUrl: './warehouse-list.component.html',
})
export class WarehouseListComponent implements OnInit {
  faFilter = faFilter;
  faSearch = faSearch;
  faPlus = faPlus;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  title: string = '';
  currentWarehouse: Warehouse = new Warehouse();
  warehouses: Array<Warehouse> = new Array<Warehouse>();
  query: FormControl = this.fb.control('');
  warehouseModal: any;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
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
    this.currentWarehouse = new Warehouse();
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

  // borrar almacén.
  public deleteWarehouse(id: string): void {
    this.authService.getMe().subscribe(async (result) => {
      if (result.role !== 'Admin') {
        await accessDenied();
      } else {
        deleteConfirm().then(result => {
          if (result.isConfirmed) {
            this.warehouseService.delete(id).subscribe(result => {
              if (result.ok) this.getWarehouses();
            });
          }
        });
      }
    });
  }

}
