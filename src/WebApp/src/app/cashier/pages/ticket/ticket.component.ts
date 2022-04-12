import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {ResponseInvoiceSale} from 'src/app/sales/interfaces';
import {InvoiceSaleService} from 'src/app/sales/services';
import {ConfigurationService} from 'src/app/system/services';
import {Configuration} from 'src/app/system/interfaces';

@Component({
  selector: 'app-ticket',
  templateUrl: './ticket.component.html'
})
export class TicketComponent implements OnInit {
  configuration: Configuration = new Configuration();
  responseInvoiceSale: ResponseInvoiceSale = new ResponseInvoiceSale();

  constructor(
    private activatedRoute: ActivatedRoute,
    private configurationService: ConfigurationService,
    private invoiceSaleService: InvoiceSaleService) {
  }

  ngOnInit(): void {
    const id: string = this.activatedRoute.snapshot.params['id'];
    this.invoiceSaleService.show(id).subscribe(result => this.responseInvoiceSale = result);
    this.configurationService.show().subscribe(result => this.configuration = result);
  }

  public print(): void {
    window.print();
  }
}
