import {Component, OnInit} from '@angular/core';
import {faEdit, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {FormBuilder, FormGroup} from '@angular/forms';
import * as moment from 'moment';
import {TransferNote} from '../../interfaces';
import {TransferNoteService} from '../../services';
import {Warehouse} from 'src/app/system/interfaces';
import {WarehouseService} from 'src/app/system/services';

declare var bootstrap: any;

@Component({
  selector: 'app-transfer-list',
  templateUrl: './transfer-list.component.html',
  styleUrls: ['./transfer-list.component.scss']
})
export class TransferListComponent implements OnInit {
  faSearch = faSearch;
  faPlus = faPlus;
  faTrashAlt = faTrashAlt;
  faEdit = faEdit;
  // TODO: debug -> $transferNotes
  transferNotes: Array<TransferNote> = new Array<TransferNote>();
  warehouses: Array<Warehouse> = new Array<Warehouse>();
  targetWarehouses: Array<Warehouse> = new Array<Warehouse>();
  // TODO: debug -> $filterForm
  filterForm: FormGroup = this.fb.group({
    origin: [localStorage.getItem('transfer_origin') || ''],
    target: [localStorage.getItem('transfer_target') || ''],
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
    this.warehouseService.index()
      .subscribe(result => {
        this.warehouses = result;
        this.changeOriginWarehouses();
        this.getTransferNotes();
      });
    // suscripción a cambios del formulario.
    this.filterForm.valueChanges.subscribe(({origin, target}) => {
      localStorage.setItem('transfer_origin', origin);
      localStorage.setItem('transfer_target', target);
    });
  }

  // cargar lista de almacenes de destino.
  public changeOriginWarehouses(): void {
    if (this.filterForm.get('origin')?.value) {
      const origin = Number(this.filterForm.get('origin')?.value);
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
    const {origin, target} = this.filterForm.value;
    if (origin === target) {
      const toastDanger = new bootstrap.Toast(document.querySelector('#toast-danger'));
      toastDanger.show();
    } else {
      this.transferNoteService.index(this.filterForm.value)
        .subscribe(result => this.transferNotes = result);
    }
  }

}
