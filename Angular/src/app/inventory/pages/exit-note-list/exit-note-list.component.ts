import {Component, OnInit} from '@angular/core';
import {faPlus, faSearch, faSignOutAlt, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {FormBuilder, FormGroup} from '@angular/forms';
import {WarehouseService} from '../../services';
import {Warehouse} from '../../interfaces';
import * as moment from 'moment';

@Component({
  selector: 'app-exit-note-list',
  templateUrl: './exit-note-list.component.html',
  styleUrls: ['./exit-note-list.component.scss']
})
export class ExitNoteListComponent implements OnInit {
  faSearch = faSearch;
  faPlus = faPlus;
  faSignOutAlt = faSignOutAlt;
  faTrashAlt = faTrashAlt;
  warehouses: Array<Warehouse> = new Array<Warehouse>();
  filterForm: FormGroup = this.fb.group({
    warehouseId: [''],
    year: [moment().format('YYYY')],
    month: [moment().format('MM')],
  });

  constructor(
    private fb: FormBuilder,
    private warehouseService: WarehouseService) {
  }

  ngOnInit(): void {
    // cargar lista de almacenes.
    this.warehouseService.index().subscribe(result => this.warehouses = result);
  }

}
