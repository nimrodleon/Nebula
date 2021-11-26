import {Component, OnInit} from '@angular/core';
import {faArrowLeft, faEdit, faIdCardAlt, faPlus, faSave, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {ActivatedRoute, Router} from '@angular/router';
import {FormBuilder, FormGroup} from '@angular/forms';
import * as moment from 'moment';
import {environment} from 'src/environments/environment';
import {InventoryReason, ItemNote, Warehouse} from '../../interfaces';
import {InventoryNoteService, InventoryReasonService, WarehouseService} from '../../services';
import Swal from 'sweetalert2';

declare var jQuery: any;
declare var bootstrap: any;

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
  noteType: string = '';
  private appURL: string = environment.applicationUrl;
  warehouses: Array<Warehouse> = new Array<Warehouse>();
  motivos: Array<InventoryReason> = new Array<InventoryReason>();
  noteForm: FormGroup = this.fb.group({
    contactId: [''],
    warehouseId: [''],
    motivo: [''],
    startDate: [moment().format('YYYY-MM-DD')],
    remark: ['']
  });
  itemNotes: Array<ItemNote> = new Array<ItemNote>();
  itemComprobanteModal: any;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private warehouseService: WarehouseService,
    private inventoryReasonService: InventoryReasonService,
    private inventoryNoteService: InventoryNoteService) {
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.noteType = params.get('type') || '';
      // cargar motivos.
      switch (this.noteType) {
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
    }).on('select2:select', (e: any) => {
      const data = e.params.data;
      this.noteForm.controls['contactId'].setValue(data.id);
    });
    // formulario item comprobante.
    this.itemComprobanteModal = new bootstrap.Modal(document.querySelector('#item-comprobante'));
  }

  // botón cancelar.
  public async cancelOption() {
    switch (this.noteType) {
      case 'input':
        await this.router.navigate(['/inventory/input-note']);
        break;
      case 'output':
        await this.router.navigate(['/inventory/output-note']);
        break;
    }
  }

  // abrir item comprobante.
  public showItemComprobanteModal(): void {
    this.itemComprobanteModal.show();
  }

  // cerrar item comprobante.
  public async hideItemComprobanteModal(data: ItemNote) {
    if (this.itemNotes.find(item =>
      item.productId === data.productId)) {
      await Swal.fire(
        'Información',
        'El producto seleccionado ya existe en la Nota!',
        'info'
      );
    } else {
      this.itemNotes.push(data);
      this.itemComprobanteModal.hide();
    }
  }

  // calcular por cantidad de producto.
  public changeQuantity(id: any, target: any): void {
    const item: ItemNote | any = this.itemNotes.find(item => item.productId === id);
    item.quantity = Number(target.value);
    item.amount = item.quantity * item.price;
  }

  // calcular por precio de producto.
  public changePrice(id: any, target: any): void {
    const item: ItemNote | any = this.itemNotes.find(item => item.productId === id);
    item.price = Number(target.value);
    item.amount = item.quantity * item.price;
  }

  // borrar item detalle nota.
  public deleteItem(id: any): void {
    this.itemNotes.forEach((value, index, array) => {
      if (value.productId === id) {
        array.splice(index, 1);
      }
    });
  }

  // botón registrar.
  public async register() {
    this.inventoryNoteService.store({
      ...this.noteForm.value, noteType: this.noteType, itemNotes: this.itemNotes
    }).subscribe(result => {
      if (result.ok) {
        this.router.navigate([`/inventory/${this.noteType}-note`]);
      }
    });
  }

}
