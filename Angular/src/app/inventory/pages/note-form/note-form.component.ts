import {Component, OnInit} from '@angular/core';
import {faArrowLeft, faEdit, faIdCardAlt, faPlus, faSave, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {ActivatedRoute, Router} from '@angular/router';
import {FormBuilder} from '@angular/forms';
import {environment} from 'src/environments/environment';
import {InventoryReason, Warehouse} from '../../interfaces';
import {InventoryReasonService, WarehouseService} from '../../services';

declare var jQuery: any;

@Component({
  selector: 'app-note-form',
  templateUrl: './note-form.component.html',
  styleUrls: ['./note-form.component.scss']
})
export class NoteFormComponent implements OnInit {
  faArrowLeft = faArrowLeft;
  faPlus = faPlus;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  faSave = faSave;
  faIdCardAlt = faIdCardAlt;
  typeNote: string = '';
  private appURL: string = environment.applicationUrl;
  warehouses: Array<Warehouse> = new Array<Warehouse>();
  motivos: Array<InventoryReason> = new Array<InventoryReason>();

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private warehouseService: WarehouseService,
    private inventoryReasonService: InventoryReasonService) {
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.typeNote = params.get('type') || '';
      // cargar motivos.
      switch (this.typeNote) {
        case 'input':
          this.inventoryReasonService.index('Input')
            .subscribe(result => this.motivos = result);
          break;
        case 'output':
          this.inventoryReasonService.index('Output')
            .subscribe(result => this.motivos = result);
          break;
      }
    });
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

  // botón cancelar.
  public async cancelOption() {
    switch (this.typeNote) {
      case 'input':
        await this.router.navigate(['/inventory/input-note']);
        break;
      case 'output':
        await this.router.navigate(['/inventory/output-note']);
        break;
    }
  }


}
