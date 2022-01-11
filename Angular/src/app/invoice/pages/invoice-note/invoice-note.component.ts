import {Component, OnInit} from '@angular/core';
import {faArrowLeft, faEdit, faIdCardAlt, faPlus, faSave, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {ActivatedRoute} from '@angular/router';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import * as moment from 'moment';
import {CpeDetail, CpeGeneric, NotaComprobante} from '../../interfaces';
import {InvoiceService} from '../../services';

declare var bootstrap: any;

@Component({
  selector: 'app-invoice-note',
  templateUrl: './invoice-note.component.html',
  styleUrls: ['./invoice-note.component.scss']
})
export class InvoiceNoteComponent implements OnInit {
  faPlus = faPlus;
  faArrowLeft = faArrowLeft;
  faSave = faSave;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  faIdCardAlt = faIdCardAlt;
  notaComprobante: NotaComprobante = new NotaComprobante();
  invoiceNoteForm: FormGroup = this.fb.group({
    startDate: [moment().format('YYYY-MM-DD'), [Validators.required]],
    docType: ['NC', [Validators.required]],
    codMotivo: ['01', [Validators.required]],
    serie: ['', [Validators.required]],
    number: ['', [Validators.required]],
    desMotivo: ['', [Validators.required]],
  });
  productId: number = 0;
  itemComprobanteModal: any;

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private invoiceService: InvoiceService) {
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.invoiceService.show(<any>params.get('invoiceId'))
        .subscribe(result => {
          if (result.invoiceType === 'VENTA') {
            this.invoiceNoteForm.controls['startDate'].disable();
            this.invoiceNoteForm.controls['serie'].disable();
            this.invoiceNoteForm.controls['number'].disable();
          }
          result.invoiceDetails.forEach(item => {
            const itemDetail = CpeGeneric.getItemDetail(item);
            itemDetail.calcularItem();
            this.notaComprobante.addItemWithData(itemDetail);
          });
        });
    });
    // item comprobante modal.
    this.itemComprobanteModal = new bootstrap.Modal(document.querySelector('#item-comprobante'));
  }

  // borrar item comprobante.
  public deleteItem(prodId: number | any): void {
    this.notaComprobante.deleteItem(prodId);
  }

  // volver una página atrás.
  public historyBack(): void {
    window.history.back();
  }

  // abrir item comprobante modal.
  public showItemComprobanteModal(): void {
    this.productId = 0;
    this.itemComprobanteModal.show();
  }

  // editar modal item-comprobante.
  public editItemComprobanteModal(id: number): void {
    this.productId = id;
    this.itemComprobanteModal.show();
  }

  // ocultar item comprobante modal.
  public hideItemComprobanteModal(data: CpeDetail): void {
    this.notaComprobante.addItemWithData(data);
    this.itemComprobanteModal.hide();
  }

  // Verificar campo invalido.
  public inputIsInvalid(field: string) {
    return this.invoiceNoteForm.controls[field].errors && this.invoiceNoteForm.controls[field].touched;
  }

  // registrar nota crédito/débito.
  public registerNote(): void {
    if (this.invoiceNoteForm.invalid) {
      this.invoiceNoteForm.markAllAsTouched();
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    this.notaComprobante = {...this.notaComprobante, ...this.invoiceNoteForm.value};
    console.log(this.notaComprobante);
  }

}
