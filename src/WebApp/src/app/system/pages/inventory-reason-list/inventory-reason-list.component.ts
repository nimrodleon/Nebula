import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {faEdit, faFilter, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {InventoryReasonService} from '../../services';
import {InventoryReason} from '../../interfaces';
import {ResponseData} from 'src/app/global/interfaces';

declare var bootstrap: any;

@Component({
  selector: 'app-inventory-reason-list',
  templateUrl: './inventory-reason-list.component.html'
})
export class InventoryReasonListComponent implements OnInit {
  faFilter = faFilter;
  faSearch = faSearch;
  faPlus = faPlus;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  title: string = '';
  currentInventoryReason: InventoryReason = new InventoryReason();
  inventoryReasons: Array<InventoryReason> = new Array<InventoryReason>();
  queryForm: FormGroup = this.fb.group({
    query: [''],
    type: ['Input']
  });
  inventoryReasonModal: any;

  constructor(
    private fb: FormBuilder,
    private inventoryReasonService: InventoryReasonService) {
  }

  ngOnInit(): void {
    // formulario motivos de inventario.
    this.inventoryReasonModal = new bootstrap.Modal(document.querySelector('#inventory-reason-modal'));
    // cargar lista de inventarios.
    this.getInventoryReasons();
  }

  // cargar lista de motivos.
  public getInventoryReasons(): void {
    const type = this.queryForm.get('type')?.value;
    const query = this.queryForm.get('query')?.value;
    this.inventoryReasonService.index(type, query)
      .subscribe(result => this.inventoryReasons = result);
  }

  // abrir modal motivos de inventario.
  public showInventoryReasonModal(): void {
    this.title = 'Agregar Motivo';
    this.currentInventoryReason = new InventoryReason();
    this.inventoryReasonModal.show();
  }

  // abrir modal motivos de inventario modo edición.
  public editInventoryReasonModal(id: any): void {
    this.title = 'Editar Motivo';
    this.inventoryReasonService.show(id).subscribe(result => {
      this.currentInventoryReason = result;
      this.inventoryReasonModal.show();
    });
  }

  // ocultar modal motivos de inventario.
  public hideInventoryReasonModal(response: ResponseData<InventoryReason>): void {
    if (response.ok) {
      this.getInventoryReasons();
      this.inventoryReasonModal.hide();
    }
  }


}
