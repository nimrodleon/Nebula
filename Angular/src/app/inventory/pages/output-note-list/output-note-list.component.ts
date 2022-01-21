import {Component, OnInit} from '@angular/core';
import {faEdit, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {FormBuilder, FormGroup} from '@angular/forms';
import * as moment from 'moment';
import {InventoryNoteService} from '../../services';
import {InventoryNote} from '../../interfaces';
import {Warehouse} from 'src/app/system/interfaces';
import {WarehouseService} from 'src/app/system/services';

@Component({
  selector: 'app-output-note-list',
  templateUrl: './output-note-list.component.html',
  styleUrls: ['./output-note-list.component.scss']
})
export class OutputNoteListComponent implements OnInit {
  faSearch = faSearch;
  faPlus = faPlus;
  faTrashAlt = faTrashAlt;
  faEdit = faEdit;
  // TODO: depurar <inventoryNotes>.
  inventoryNotes: Array<InventoryNote> = new Array<InventoryNote>();
  warehouses: Array<Warehouse> = new Array<Warehouse>();
  // TODO: depurar <filterForm>.
  filterForm: FormGroup = this.fb.group({
    warehouseId: [localStorage.getItem('warehouse_output_note') || ''],
    year: [moment().format('YYYY')],
    month: [moment().format('MM')],
  });

  constructor(
    private fb: FormBuilder,
    private warehouseService: WarehouseService,
    private inventoryNoteService: InventoryNoteService) {
  }

  ngOnInit(): void {
    // cargar lista de almacenes.
    this.warehouseService.index().subscribe(result => this.warehouses = result);
    // guardar almacén seleccionado.
    this.filterForm.valueChanges.subscribe(({warehouseId}) => {
      localStorage.setItem('warehouse_output_note', warehouseId);
    });
    // cargar lista de notas.
    this.getInventoryNotes();
  }

  // cargar lista de notas.
  public getInventoryNotes(): void {
    this.inventoryNoteService.index({...this.filterForm.value, noteType: 'OUTPUT'})
      .subscribe(result => this.inventoryNotes = result);
  }

}
