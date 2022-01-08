import {Component, OnInit} from '@angular/core';
import {faArrowLeft, faEdit, faIdCardAlt, faPlus, faSave, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {ActivatedRoute} from '@angular/router';
import {FormBuilder, FormGroup} from '@angular/forms';
import * as moment from 'moment';
import {CpeDetail, CpeGeneric, Invoice, InvoiceDetail, NotaComprobante} from '../../interfaces';
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
  invoiceNote: FormGroup = this.fb.group({
    startDate: [moment().format('YYYY-MM-DD')],
    docType: ['NC'],
    codMotivo: ['01'],
    serie: [''],
    number: [''],
    desMotivo: [''],
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

}
