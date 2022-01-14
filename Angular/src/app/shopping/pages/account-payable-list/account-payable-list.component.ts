import {Component, OnInit} from '@angular/core';
import {faCoins, faEdit, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {FormBuilder, FormGroup} from '@angular/forms';
import * as moment from 'moment';
import {InvoiceAccountService} from 'src/app/invoice/services';
import {InvoiceAccount} from 'src/app/invoice/interfaces';

@Component({
  selector: 'app-account-payable-list',
  templateUrl: './account-payable-list.component.html',
  styleUrls: ['./account-payable-list.component.scss']
})
export class AccountPayableListComponent implements OnInit {
  faSearch = faSearch;
  faTrashAlt = faTrashAlt;
  faCoins = faCoins;
  faEdit = faEdit;
  invoiceAccounts: Array<InvoiceAccount> = new Array<InvoiceAccount>();
  queryForm: FormGroup = this.fb.group({
    year: [moment().format('YYYY')],
    month: [moment().format('MM')],
    query: ['PENDIENTE']
  });

  constructor(
    private fb: FormBuilder,
    private invoiceAccountService: InvoiceAccountService) {
  }

  ngOnInit(): void {
    this.submitQuery();
  }

  // buscar cuentas por pagar.
  public submitQuery(): void {
    this.invoiceAccountService.index('PAGAR', this.queryForm.value)
      .subscribe(result => this.invoiceAccounts = result);
  }

  public diasMora(fecha: any): number {
    const date = new Date();
    fecha = moment(fecha).toDate();
    const fechaActual = moment([
      date.getFullYear(), date.getMonth(), date.getDate()
    ]);
    const dias = fechaActual.diff(moment([
      fecha.getFullYear(), fecha.getMonth(), fecha.getDate()
    ]), 'days');
    return dias > 0 ? dias : 0;
  }

}
