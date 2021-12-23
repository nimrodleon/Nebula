import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {faEnvelope, faPrint, faStickyNote, faTimes} from '@fortawesome/free-solid-svg-icons';
import {environment} from 'src/environments/environment';
import {InvoiceService} from '../../services';
import {Invoice} from '../../interfaces';

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
  faStickyNote = faStickyNote;
  invoice: Invoice = new Invoice();

  constructor(
    private activatedRoute: ActivatedRoute,
    private invoiceService: InvoiceService) {
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.invoiceService.show(<any>params.get('id'))
        .subscribe(result => {
          this.invoice = result;
          this.urlPdf = `${this.appURL}Facturador/GetPdf?invoice=${result.id}`;
        });
    });
  }

  public getNameInvoice(value: string): string {
    let result: string = '';
    switch (value) {
      case 'FT':
        result = 'FACTURA';
        break;
      case 'BL':
        result = 'BOLETA';
        break;
      case 'NV':
        result = 'NOTA DE VENTA';
        break;
    }
    return result;
  }

}
