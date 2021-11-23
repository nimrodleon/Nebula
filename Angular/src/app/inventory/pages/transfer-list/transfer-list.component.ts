import {Component, OnInit} from '@angular/core';
import {faPlus, faSearch, faSignOutAlt, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {FormBuilder, FormGroup} from '@angular/forms';
import * as moment from 'moment';
import {Warehouse} from '../../interfaces';
import {WarehouseService} from '../../services';

@Component({
  selector: 'app-transfer-list',
  templateUrl: './transfer-list.component.html',
  styleUrls: ['./transfer-list.component.scss']
})
export class TransferListComponent implements OnInit {
  faSearch = faSearch;
  faPlus = faPlus;
  faSignOutAlt = faSignOutAlt;
  faTrashAlt = faTrashAlt;
  warehouses: Array<Warehouse> = new Array<Warehouse>();
  targetWarehouses: Array<Warehouse> = new Array<Warehouse>();
  filterForm: FormGroup = this.fb.group({
    origin: [''],
    target: [''],
    year: [moment().format('YYYY')],
    month: [moment().format('MM')]
  });

  constructor(
    private fb: FormBuilder,
    private warehouseService: WarehouseService) {
  }

  ngOnInit(): void {
    // cargar almacenes de origen.
    this.warehouseService.index().subscribe(result => this.warehouses = result);
  }

  // cargar lista de almacenes de destino.
  public changeOriginWarehouses(): void {
    if (this.filterForm.get('origin')?.value) {
      const origin = this.filterForm.get('origin')?.value;
      this.targetWarehouses = new Array<Warehouse>();
      this.warehouses.forEach(item => {
        if (item.id !== origin) {
          this.targetWarehouses.push(item);
        }
      });
    }
  }

}
