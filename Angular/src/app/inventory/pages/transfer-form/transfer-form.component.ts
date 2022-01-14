import {Component, OnInit} from '@angular/core';
import {faArrowLeft, faEdit, faIdCardAlt, faPlus, faSave, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import * as moment from 'moment';
import Swal from 'sweetalert2';
import {ItemNote} from '../../interfaces';
import {TransferNoteService} from '../../services';
import {InventoryReason, Warehouse} from 'src/app/system/interfaces';
import {InventoryReasonService, WarehouseService} from 'src/app/system/services';
import {CpeDetail} from 'src/app/invoice/interfaces';
import {confirmTask} from '../../../global/interfaces';

declare var bootstrap: any;

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
  transferId: number | null = null;
  warehouses: Array<Warehouse> = new Array<Warehouse>();
  targetWarehouses: Array<Warehouse> = new Array<Warehouse>();
  motivos: Array<InventoryReason> = new Array<InventoryReason>();
  transferForm: FormGroup = this.fb.group({
    origin: ['', [Validators.required]],
    target: ['', [Validators.required]],
    motivo: ['', [Validators.required]],
    startDate: [moment().format('YYYY-MM-DD'), [Validators.required]]
  });
  itemNotes: Array<ItemNote> = new Array<ItemNote>();
  itemComprobanteModal: any;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private warehouseService: WarehouseService,
    private inventoryReasonService: InventoryReasonService,
    private transferNoteService: TransferNoteService) {
  }

  ngOnInit(): void {
    // cargar lista de almacenes.
    this.warehouseService.index().subscribe(result => {
      this.warehouses = result;
      // configurar modo edición.
      this.activatedRoute.paramMap.subscribe(params => {
        if (params.get('id')) {
          this.transferNoteService.show(<any>params.get('id'))
            .subscribe(result => {
              this.transferId = Number(params.get('id'));
              this.transferForm.controls['origin'].setValue(result.originId);
              // cargar lista de almacenes de destino.
              this.warehouses.forEach(item => {
                if (item.id !== result.originId) {
                  this.targetWarehouses.push(item);
                }
              });
              this.transferForm.controls['target'].setValue(result.targetId);
              this.transferForm.controls['motivo'].setValue(result.motivo.split('|')[0]);
              this.transferForm.controls['startDate'].setValue(moment(result.startDate).format('YYYY-MM-DD'));
              // cargar detalle nota transferencia entre almacenes.
              result.transferNoteDetails.forEach(item => {
                this.itemNotes.push(new ItemNote(item.productId, item.description, item.quantity, item.price, item.amount));
              });
            });
        }
      });
    });
    // cargar lista de motivos.
    this.inventoryReasonService.index('Transfer').subscribe(result => this.motivos = result);
    // formulario item comprobante.
    this.itemComprobanteModal = new bootstrap.Modal(document.querySelector('#item-comprobante'));
  }

  // cargar lista de almacenes de destino.
  public changeOriginWarehouse(): void {
    if (this.transferForm.get('origin')?.value) {
      const origin = Number(this.transferForm.get('origin')?.value);
      this.targetWarehouses = new Array<Warehouse>();
      this.warehouses.forEach(item => {
        if (item.id !== origin) {
          this.targetWarehouses.push(item);
        }
      });
    }
  }

  // cargar item comprobante.
  public showItemComprobanteModal(): void {
    this.itemComprobanteModal.show();
  }

  // cerrar item comprobante.
  public async hideItemComprobanteModal(data: CpeDetail) {
    if (this.itemNotes.find(item =>
      item.productId === data.productId)) {
      await Swal.fire(
        'Información',
        'El producto seleccionado ya existe en la Nota!',
        'info'
      );
    } else {
      this.itemNotes.push(new ItemNote(data.productId, data.description, data.quantity, data.price, data.amount));
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
    return this.transferForm.controls[field].errors && this.transferForm.controls[field].touched;
  }

  // botón registrar.
  public async register() {
    if (this.transferForm.invalid) {
      this.transferForm.markAllAsTouched();
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    confirmTask().then(result => {
      if (result.isConfirmed) {
        if (this.transferId === null) {
          this.transferNoteService.create({
            ...this.transferForm.value, itemNotes: this.itemNotes
          }).subscribe(result => {
            if (result.ok) {
              this.router.navigate(['/inventory/transfer']);
            }
          });
        } else {
          this.transferNoteService.update(this.transferId, {
            ...this.transferForm.value, itemNotes: this.itemNotes
          }).subscribe(result => {
            if (result.ok) {
              this.router.navigate(['/inventory/transfer']);
            }
          });
        }
      }
    });
  }

  // volver una página atrás.
  public historyBack(): void {
    window.history.back();
  }

}
