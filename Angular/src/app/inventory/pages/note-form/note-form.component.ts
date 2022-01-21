import {Component, OnInit} from '@angular/core';
import {faArrowLeft, faEdit, faIdCardAlt, faPlus, faSave, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {ActivatedRoute, Router} from '@angular/router';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import * as moment from 'moment';
import Swal from 'sweetalert2';
import {environment} from 'src/environments/environment';
import {ItemNote} from '../../interfaces';
import {InventoryNoteService} from '../../services';
import {InventoryReason, Warehouse} from 'src/app/system/interfaces';
import {InventoryReasonService, WarehouseService} from 'src/app/system/services';
import {confirmTask} from '../../../global/interfaces';

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
  // TODO: debug -> $noteType
  noteType: string = '';
  private appURL: string = environment.applicationUrl;
  noteId: number | null = null;
  warehouses: Array<Warehouse> = new Array<Warehouse>();
  motivos: Array<InventoryReason> = new Array<InventoryReason>();
  // TODO: debug -> $noteForm
  noteForm: FormGroup = this.fb.group({
    contactId: ['', [Validators.required]],
    warehouseId: ['', [Validators.required]],
    motivo: ['', [Validators.required]],
    startDate: [moment().format('YYYY-MM-DD'), [Validators.required]]
  });
  // TODO: debug -> $itemNotes
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
      if (params.get('id')) {
        this.noteId = Number(params.get('id'));
        this.inventoryNoteService.show(<any>params.get('id')).subscribe(result => {
          // cargar cliente a select2.
          const newOption = new Option(result.contact.name, result.contact.id, true, true);
          jQuery('#contactId').append(newOption).trigger('change');
          this.noteForm.controls['contactId'].setValue(result.contactId);
          this.noteForm.controls['warehouseId'].setValue(result.warehouseId);
          this.noteForm.controls['motivo'].setValue(result.motivo.split('|')[0]);
          this.noteForm.controls['startDate'].setValue(moment(result.startDate).format('YYYY-MM-DD'));
          // cargar items nota de inventario.
          result.inventoryNoteDetails.forEach(item => {
            this.itemNotes.push({
              productId: item.productId,
              description: item.description,
              quantity: item.quantity,
              price: item.price,
              amount: item.amount
            });
          });
        });
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

  // Verificar campo invalido.
  public inputIsInvalid(field: string) {
    return this.noteForm.controls[field].errors && this.noteForm.controls[field].touched;
  }

  // botón registrar.
  public async register() {
    if (this.noteForm.invalid) {
      this.noteForm.markAllAsTouched();
      return;
    }
    // validar detalle de transferencia.
    if (this.itemNotes.length <= 0) {
      await Swal.fire(
        'Información',
        'Debe existir al menos un Item para registrar!',
        'info'
      );
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    confirmTask().then(result => {
      if (result.isConfirmed) {
        if (this.noteId === null) {
          this.inventoryNoteService.store({
            ...this.noteForm.value, noteType: this.noteType, itemNotes: this.itemNotes
          }).subscribe(result => {
            if (result.ok) {
              this.router.navigate([`/inventory/${this.noteType}-note`]);
            }
          });
        } else {
          this.inventoryNoteService.update(this.noteId, {
            ...this.noteForm.value, noteType: this.noteType, itemNotes: this.itemNotes
          }).subscribe(result => {
            if (result.ok) {
              this.router.navigate([`/inventory/${this.noteType}-note`]);
            }
          });
        }
      }
    });
  }

}
