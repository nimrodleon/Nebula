import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {faBars} from '@fortawesome/free-solid-svg-icons';
import {InvoiceSerieService, WarehouseService} from '../../services';
import {InvoiceSerie, Warehouse} from '../../interfaces';
import {ResponseData} from 'src/app/global/interfaces';

@Component({
  selector: 'app-invoice-serie-modal',
  templateUrl: './invoice-serie-modal.component.html',
  styleUrls: ['./invoice-serie-modal.component.scss']
})
export class InvoiceSerieModalComponent implements OnInit {
  @Input()
  title: string = '';
  @Input()
  invoiceSerie: InvoiceSerie = new InvoiceSerie();
  @Output()
  responseData = new EventEmitter<ResponseData<InvoiceSerie>>();
  faBars = faBars;
  warehouses: Array<Warehouse> = new Array<Warehouse>();
  invoiceSerieForm: FormGroup = this.fb.group({
    id: [null],
    name: ['', [Validators.required]],
    warehouse: [null, [Validators.required]],
    factura: ['', [Validators.required]],
    counterFactura: [0, [Validators.required, Validators.min(0)]],
    boleta: ['', [Validators.required]],
    counterBoleta: [0, [Validators.required, Validators.min(0)]],
    notaDeVenta: ['', [Validators.required]],
    counterNotaDeVenta: [0, [Validators.required, Validators.min(0)]]
  });

  constructor(
    private fb: FormBuilder,
    private warehouseService: WarehouseService,
    private invoiceSerieService: InvoiceSerieService) {
  }

  ngOnInit(): void {
    // cargar lista de almacenes.
    this.warehouseService.index().subscribe(result => this.warehouses = result);
    // cargar valores por defecto.
    const myModal: any = document.querySelector('#invoice-serie-modal');
    myModal.addEventListener('shown.bs.modal', () => {
      if (this.invoiceSerie !== null) {
        this.invoiceSerieForm.reset(this.invoiceSerie);
      }
    });
    myModal.addEventListener('hide.bs.modal', () => {
      this.invoiceSerieForm.addControl('id', new FormControl(null));
      this.invoiceSerieForm.reset({counterFactura: 0, counterBoleta: 0, counterNotaDeVenta: 0});
    });
  }

  // Verificar campo invalido.
  public inputIsInvalid(field: string) {
    return this.invoiceSerieForm.controls[field].errors && this.invoiceSerieForm.controls[field].touched;
  }

  // guardar cambios.
  public saveChanges(): void {
    if (this.invoiceSerieForm.invalid) {
      this.invoiceSerieForm.markAllAsTouched();
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    if (this.invoiceSerieForm.get('id')?.value === null) {
      this.invoiceSerieForm.removeControl('id');
      this.invoiceSerieService.create(this.invoiceSerieForm.value)
        .subscribe(result => this.responseData.emit(result));
    } else {
      const id = this.invoiceSerieForm.get('id')?.value;
      this.invoiceSerieService.update(id, this.invoiceSerieForm.value)
        .subscribe(result => this.responseData.emit(result));
    }
  }

}
