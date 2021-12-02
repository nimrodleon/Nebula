import {Component, OnInit} from '@angular/core';
import {faPlus, faSearch, faSignOutAlt, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {FormBuilder, FormGroup} from '@angular/forms';
import * as moment from 'moment';
import {TransferNote} from '../../interfaces';
import {TransferNoteService} from '../../services';
import {Warehouse} from 'src/app/system/interfaces';
import {WarehouseService} from 'src/app/system/services';

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
  transferNotes: Array<TransferNote> = new Array<TransferNote>();
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
    private warehouseService: WarehouseService,
    private transferNoteService: TransferNoteService) {
  }

  ngOnInit(): void {
    // cargar almacenes de origen.
    this.warehouseService.index().subscribe(result => this.warehouses = result);
    // cargar lista de transferencias.
    this.getTransferNotes();
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

  // cargar lista de transferencias.
  public getTransferNotes(): void {
    this.transferNoteService.index(this.filterForm.value)
      .subscribe(result => this.transferNotes = result);
  }

}
