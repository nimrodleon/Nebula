import {Component, OnInit} from '@angular/core';
import {faArrowLeft, faEdit, faIdCardAlt, faPlus, faSave, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {ActivatedRoute} from '@angular/router';
import {FormBuilder, FormGroup} from '@angular/forms';
import * as moment from 'moment';
import {DetailComprobante, Invoice, InvoiceDetail} from '../../interfaces';
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
  invoice: Invoice = new Invoice();
  invoiceNote: FormGroup = this.fb.group({
    invoiceId: [''],
    startDate: [moment().format('YYYY-MM-DD')],
    docType: ['NC'],
    codMotivo: ['01'],
    serie: [''],
    number: [''],
    desMotivo: [''],
    sumTotTributos: [0],
    sumTotValVenta: [0],
    sumImpVenta: [0],
    remark: ['']
  });
  invoiceNoteDetail: Array<DetailComprobante> = new Array<DetailComprobante>();
  itemComprobanteModal: any;

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private invoiceService: InvoiceService) {
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.invoiceService.show(<any>params.get('id'))
        .subscribe(result => {
          this.invoice = result;
          this.invoiceNote.controls['invoiceId'].setValue(result.id);
          this.invoiceNote.controls['serie'].setValue(result.serie);
          this.invoiceNote.controls['number'].setValue(result.number);
          // subTotal igv y total factura/boleta.
          this.invoiceNote.controls['sumTotTributos'].setValue(result.sumTotTributos);
          this.invoiceNote.controls['sumTotValVenta'].setValue(result.sumTotValVenta);
          this.invoiceNote.controls['sumImpVenta'].setValue(result.sumImpVenta);
          // detalle notas de crédito/débito.
          result.invoiceDetails.forEach((value: InvoiceDetail, index: number) => {
            this.invoiceNoteDetail.push({
              numItem: index + 1, productId: Number(value.codProducto),
              description: value.desItem, price: value.mtoValorUnitario, quantity: value.ctdUnidadItem,
              amount: value.mtoValorVentaItem
            });
          });
        });
    });
    // item comprobante modal.
    this.itemComprobanteModal = new bootstrap.Modal(document.querySelector('#item-comprobante'));
  }

  // abrir item comprobante modal.
  public showItemComprobanteModal(): void {
    this.itemComprobanteModal.show();
  }

  // ocultar item comprobante modal.
  public hideItemComprobanteModal(data: DetailComprobante): void {

  }

}
