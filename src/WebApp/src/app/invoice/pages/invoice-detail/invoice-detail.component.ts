import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {faEdit, faEnvelope, faPrint, faStickyNote, faTimes, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {environment} from 'src/environments/environment';
import {InvoiceSaleService} from '../../services';
import {InvoiceSale, InvoiceNote} from '../../interfaces';

@Component({
  selector: 'app-invoice-detail',
  templateUrl: './invoice-detail.component.html',
  styleUrls: ['./invoice-detail.component.scss']
})
export class InvoiceDetailComponent implements OnInit {
  private appURL: string = environment.applicationUrl;
  urlPdf: string = '';
  faTimes = faTimes;
  faEnvelope = faEnvelope;
  faPrint = faPrint;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  faStickyNote = faStickyNote;
  invoice: InvoiceSale = new InvoiceSale();
  invoiceNotes: Array<InvoiceNote> = new Array<InvoiceNote>();
  // TODO: debug -> $invoiceType
  invoiceType: string = '';

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private invoiceService: InvoiceSaleService) {
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.invoiceType = params.get('type') || '';
      this.invoiceService.show(<any>params.get('id'))
        .subscribe(result => {
          this.invoice = result;
          this.urlPdf = `${this.appURL}Facturador/GetPdf?invoice=${result.id}`;
        });
      // this.invoiceService.notes(<any>params.get('id'))
      //   .subscribe(result => this.invoiceNotes = result);
    });
  }

  // Imprimir comprobante.
  public async print() {
    // if (this.invoice.invoiceType === 'COMPRA') window.print();
    // if (this.invoice.invoiceType === 'VENTA') await this.router.navigate([this.urlPdf]);
  }

}
