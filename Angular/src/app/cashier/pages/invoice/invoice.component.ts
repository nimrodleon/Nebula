import {Component, OnInit} from '@angular/core';
import {faEnvelope, faPrint, faTimes} from '@fortawesome/free-solid-svg-icons';
import {ActivatedRoute} from '@angular/router';
import {Invoice} from 'src/app/invoice/interfaces';
import {InvoiceService} from 'src/app/invoice/services';

@Component({
  selector: 'app-invoice',
  templateUrl: './invoice.component.html',
  styleUrls: ['./invoice.component.scss']
})
export class InvoiceComponent implements OnInit {
  faTimes = faTimes;
  faEnvelope = faEnvelope;
  faPrint = faPrint;
  invoice: Invoice = new Invoice();

  constructor(
    private activatedRoute: ActivatedRoute,
    private invoiceService: InvoiceService) {
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.invoiceService.show(<any>params.get('id'))
        .subscribe(result => this.invoice = result);
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
