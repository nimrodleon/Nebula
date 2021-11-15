import {Component, OnInit} from '@angular/core';
import {faEdit, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {FormBuilder, FormGroup} from '@angular/forms';
import * as moment from 'moment';
import {Invoice} from 'src/app/invoice/interfaces';
import {InvoiceService} from 'src/app/invoice/services';

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
  invoices: Array<Invoice> = new Array<Invoice>();
  queryForm: FormGroup = this.fb.group({
    year: [moment().format('YYYY')],
    month: [moment().format('MM')],
    query: ['']
  });

  constructor(
    private fb: FormBuilder,
    private invoiceService: InvoiceService) {
  }

  ngOnInit(): void {
    this.submitQuery();
  }

  // buscar comprobantes.
  public submitQuery(): void {
    this.invoiceService.index('PURCHASE', this.queryForm.value)
      .subscribe(result => this.invoices = result);
  }

}
