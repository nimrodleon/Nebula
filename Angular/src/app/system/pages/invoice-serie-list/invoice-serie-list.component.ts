import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl} from '@angular/forms';
import {faEdit, faFilter, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {InvoiceSerie} from '../../interfaces';
import {InvoiceSerieService} from '../../services';
import {ResponseData} from '../../../global/interfaces';

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
  currentInvoiceSerie: InvoiceSerie = new InvoiceSerie();
  invoiceSeries: Array<InvoiceSerie> = new Array<InvoiceSerie>();
  invoiceSerieModal: any;
  query: FormControl = this.fb.control('');

  constructor(
    private fb: FormBuilder,
    private invoiceSerieService: InvoiceSerieService) {
  }

  ngOnInit(): void {
    // modal serie facturación.
    this.invoiceSerieModal = new bootstrap.Modal(document.querySelector('#invoice-serie-modal'));
    // cargar lista de series de facturación al iniciar el componente.
    this.getInvoiceSeries();
  }

  // cargar lista de series.
  private getInvoiceSeries(): void {
    this.invoiceSerieService.index(this.query.value)
      .subscribe(result => this.invoiceSeries = result);
  }

  // formulario buscar series.
  public submitSearch(e: Event): void {
    e.preventDefault();
    this.getInvoiceSeries();
  }

  // abrir modal serie facturación.
  public showInvoiceSerieModal(): void {
    this.title = 'Agregar Serie';
    this.currentInvoiceSerie = new InvoiceSerie();
    this.invoiceSerieModal.show();
  }

  // abrir modal serie facturación modo edición.
  public editInvoiceSerieModal(id: any): void {
    this.title = 'Editar Serie';
    this.invoiceSerieService.show(id).subscribe(result => {
      this.currentInvoiceSerie = result;
      this.invoiceSerieModal.show();
    });
  }

  // ocultar modal serie facturación.
  public hideInvoiceSerieModal(response: ResponseData<InvoiceSerie>): void {
    if (response.ok) {
      this.getInvoiceSeries();
      this.invoiceSerieModal.hide();
    }
  }

}
