import {Component, OnInit} from '@angular/core';
import {faEdit, faFilter, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';

declare var bootstrap: any;

@Component({
  selector: 'app-invoice-serie-list',
  templateUrl: './invoice-serie-list.component.html',
  styleUrls: ['./invoice-serie-list.component.scss']
})
export class InvoiceSerieListComponent implements OnInit {
  faFilter = faFilter;
  faSearch = faSearch;
  faPlus = faPlus;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  title: string = '';
  invoiceSerieModal: any;

  constructor() {
  }

  ngOnInit(): void {
    // modal serie facturación.
    this.invoiceSerieModal = new bootstrap.Modal(document.querySelector('#invoice-serie-modal'));
  }

  // abrir modal serie facturación.
  public showInvoiceSerieModal(): void {
    this.title = 'Agregar Serie';
    this.invoiceSerieModal.show();
  }

}
