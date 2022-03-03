import {Component, OnInit} from '@angular/core';
import {faEdit, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {FormBuilder, FormGroup} from '@angular/forms';
import * as moment from 'moment';
import {InvoiceSale} from 'src/app/invoice/interfaces';
import {InvoiceSaleService} from 'src/app/invoice/services';

@Component({
  selector: 'app-shopping-list',
  templateUrl: './shopping-list.component.html',
  styleUrls: ['./shopping-list.component.scss']
})
export class ShoppingListComponent implements OnInit {
  faSearch = faSearch;
  faPlus = faPlus;
  faTrashAlt = faTrashAlt;
  faEdit = faEdit;
  // TODO: debug -> $invoices
  invoices: Array<InvoiceSale> = new Array<InvoiceSale>();
  // TODO: debug -> $queryForm
  queryForm: FormGroup = this.fb.group({
    year: [moment().format('YYYY')],
    month: [moment().format('MM')],
    query: ['']
  });

  constructor(
    private fb: FormBuilder,
    private invoiceService: InvoiceSaleService) {
  }

  ngOnInit(): void {
    this.submitQuery();
  }

  // buscar comprobantes.
  public submitQuery(): void {
    this.invoiceService.index('COMPRA', this.queryForm.value)
      .subscribe(result => this.invoices = result);
  }

}
