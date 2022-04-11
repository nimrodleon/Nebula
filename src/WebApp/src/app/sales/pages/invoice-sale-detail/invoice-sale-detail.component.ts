import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {faEdit, faPrint, faTimes, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {InvoiceSaleService} from '../../services';
import {ResponseInvoiceSale} from '../../interfaces';

@Component({
  selector: 'app-invoice-sale-detail',
  templateUrl: './invoice-sale-detail.component.html',
  styleUrls: ['./invoice-sale-detail.component.scss']
})
export class InvoiceSaleDetailComponent implements OnInit {
  faTimes = faTimes;
  faPrint = faPrint;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  public responseInvoiceSale: ResponseInvoiceSale = new ResponseInvoiceSale();

  constructor(
    private activatedRoute: ActivatedRoute,
    private invoiceSaleService: InvoiceSaleService) {
  }

  ngOnInit(): void {
    const id: string = this.activatedRoute.snapshot.params['id'];
    this.invoiceSaleService.show(id).subscribe(result => this.responseInvoiceSale = result);
  }

  public print(): void {
    window.print();
  }

}
