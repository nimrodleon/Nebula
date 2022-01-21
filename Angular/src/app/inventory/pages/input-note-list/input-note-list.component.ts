import {Component, OnInit} from '@angular/core';
import {faEdit, faIdCardAlt, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {FormBuilder, FormGroup} from '@angular/forms';
import * as moment from 'moment';
import {InventoryNoteService} from '../../services';
import {InventoryNote} from '../../interfaces';
import {Warehouse} from 'src/app/system/interfaces';
import {WarehouseService} from 'src/app/system/services';

@Component({
  selector: 'app-input-note-list',
  templateUrl: './input-note-list.component.html',
  styleUrls: ['./input-note-list.component.scss']
})
export class InputNoteListComponent implements OnInit {
  faSearch = faSearch;
  faPlus = faPlus;
  faTrashAlt = faTrashAlt;
  faIdCardAlt = faIdCardAlt;
  faEdit = faEdit;
  // TODO: depurar <inventoryNotes>.
  inventoryNotes: Array<InventoryNote> = new Array<InventoryNote>();
  warehouses: Array<Warehouse> = new Array<Warehouse>();
  // TODO: depurar <filterForm>.
  filterForm: FormGroup = this.fb.group({
    warehouseId: [localStorage.getItem('warehouse_input_note') || ''],
    year: [moment().format('YYYY')],
    month: [moment().format('MM')],
  });

  constructor(
    private fb: FormBuilder,
    private warehouseService: WarehouseService,
    private inventoryNoteService: InventoryNoteService) {
  }

  ngOnInit(): void {
    this.filterForm.valueChanges.subscribe(({warehouseId}) => {
      localStorage.setItem('warehouse_input_note', warehouseId);
    });
    // cargar lista de almacenes.
    this.warehouseService.index().subscribe(result => this.warehouses = result);
    // cargar lista de notas.
    this.getInventoryNotes();
  }

  // cargar lista de notas.
  public getInventoryNotes(): void {
    const {warehouseId} = this.filterForm.value;
    if (warehouseId) {
      this.inventoryNoteService.index({...this.filterForm.value, noteType: 'INPUT'})
        .subscribe(result => this.inventoryNotes = result);
    }
  }

}
