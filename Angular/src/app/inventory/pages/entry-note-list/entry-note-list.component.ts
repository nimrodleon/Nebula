import {Component, OnInit} from '@angular/core';
import {faIdCardAlt, faPlus, faSearch, faSignOutAlt, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {FormBuilder} from '@angular/forms';
import {WarehouseService} from '../../services';
import {Warehouse} from '../../interfaces';

@Component({
  selector: 'app-entry-note-list',
  templateUrl: './entry-note-list.component.html',
  styleUrls: ['./entry-note-list.component.scss']
})
export class EntryNoteListComponent implements OnInit {
  faSearch = faSearch;
  faPlus = faPlus;
  faSignOutAlt = faSignOutAlt;
  faTrashAlt = faTrashAlt;
  faIdCardAlt = faIdCardAlt;
  warehouses: Array<Warehouse> = new Array<Warehouse>();

  constructor(
    private fb: FormBuilder,
    private warehouseService: WarehouseService) {
  }

  ngOnInit(): void {
    // cargar lista de almacenes.
    this.warehouseService.index().subscribe(result => this.warehouses = result);
  }

}
