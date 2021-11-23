import {Component, OnInit} from '@angular/core';
import {faArrowLeft, faEdit, faIdCardAlt, faPlus, faSave, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {Warehouse} from '../../interfaces';
import {FormBuilder, FormGroup} from '@angular/forms';
import {WarehouseService} from '../../services';
import * as moment from 'moment';

@Component({
  selector: 'app-transfer-form',
  templateUrl: './transfer-form.component.html',
  styleUrls: ['./transfer-form.component.scss']
})
export class TransferFormComponent implements OnInit {
  faArrowLeft = faArrowLeft;
  faPlus = faPlus;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  faSave = faSave;
  faIdCardAlt = faIdCardAlt;
  warehouses: Array<Warehouse> = new Array<Warehouse>();
  targetWarehouses: Array<Warehouse> = new Array<Warehouse>();
  transferForm: FormGroup = this.fb.group({
    origin: [''],
    target: [''],
    motivo: [''],
    startDate: [moment().format('YYYY-MM-DD')],
    remark: ['']
  });

  constructor(
    private fb: FormBuilder,
    private warehouseService: WarehouseService) {
  }

  ngOnInit(): void {
    // cargar lista de almacenes.
    this.warehouseService.index().subscribe(result => this.warehouses = result);
  }

  // cargar lista de almacenes de destino.
  public changeOriginWarehouse(): void {
    if (this.transferForm.get('origin')?.value) {
      const origin = this.transferForm.get('origin')?.value;
      this.targetWarehouses = new Array<Warehouse>();
      this.warehouses.forEach(item => {
        if (item.id !== origin) {
          this.targetWarehouses.push(item);
        }
      });
    }
  }

}
