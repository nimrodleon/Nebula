import {Component, OnInit} from '@angular/core';
import {faIdCardAlt, faPlus, faSearch, faSignOutAlt, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {FormBuilder} from '@angular/forms';
import {environment} from 'src/environments/environment';
import {WarehouseService} from '../../services';
import {Warehouse} from '../../interfaces';

declare var jQuery: any;

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
  faIdCardAlt = faIdCardAlt;
  private appURL: string = environment.applicationUrl;
  warehouses: Array<Warehouse> = new Array<Warehouse>();

  constructor(
    private fb: FormBuilder,
    private warehouseService: WarehouseService) {
  }

  ngOnInit(): void {
    // cargar lista de almacenes.
    this.warehouseService.index().subscribe(result => this.warehouses = result);
    // buscador de contactos.
    jQuery('#contactId').select2({
      theme: 'bootstrap-5',
      placeholder: 'BUSCAR CONTACTO',
      ajax: {
        url: this.appURL + 'Contact/Select2',
        headers: {
          Authorization: 'Bearer ' + localStorage.getItem('token')
        }
      }
    });
  }

}
