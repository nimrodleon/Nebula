import {Component, OnInit} from '@angular/core';
import {faCog, faEdit, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {FormBuilder, FormGroup} from '@angular/forms';
import * as moment from 'moment';
import {InvoiceService} from 'src/app/invoice/services';
import {Invoice} from 'src/app/invoice/interfaces';

@Component({
  selector: 'app-sales-list',
  templateUrl: './sales-list.component.html',
  styleUrls: ['./sales-list.component.scss']
})
export class SalesListComponent implements OnInit {
  faSearch = faSearch;
  faPlus = faPlus;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  faCog = faCog;
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
    this.invoiceService.index('SALE', this.queryForm.value)
      .subscribe(result => this.invoices = result);
  }

}
