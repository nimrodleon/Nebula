import {Component} from "@angular/core";
import {ActivatedRoute} from "@angular/router";
import {faEdit, faFilter, faPlus, faSearch, faTrashAlt} from "@fortawesome/free-solid-svg-icons";
import {FormBuilder, FormControl, ReactiveFormsModule} from "@angular/forms";
import {deleteConfirm, toastSuccess} from "app/common/interfaces";
import {InvoiceSerieService} from "../../services";
import {InvoiceSerie, InvoiceSerieDataModal} from "../../interfaces";
import _ from "lodash";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {
  CompanyDetailContainerComponent
} from "app/common/containers/company-detail-container/company-detail-container.component";
import {InvoiceSerieModalComponent} from "../../components/invoice-serie-modal/invoice-serie-modal.component";
import {NgForOf} from "@angular/common";

declare const bootstrap: any;

@Component({
  selector: "app-invoice-serie-list",
  standalone: true,
  imports: [
    FaIconComponent,
    CompanyDetailContainerComponent,
    ReactiveFormsModule,
    InvoiceSerieModalComponent,
    NgForOf
  ],
  templateUrl: "./invoice-serie-list.component.html"
})
export class InvoiceSerieListComponent {
  faSearch = faSearch;
  faPlus = faPlus;
  faFilter = faFilter;
  faTrashAlt = faTrashAlt;
  faEdit = faEdit;
  companyId: string = "";
  invoiceSeries: Array<InvoiceSerie> = new Array<InvoiceSerie>();
  invoiceSerieModal: any;
  query: FormControl = this.fb.control("");
  invoiceSerieDataModal = new InvoiceSerieDataModal();

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private invoiceSerieService: InvoiceSerieService) {
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
      // cargar lista de series de facturación al iniciar el componente.
      this.getInvoiceSeries();
    });
    // modal serie facturación.
    this.invoiceSerieModal = new bootstrap.Modal("#invoice-serie-modal");
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
  public addInvoiceSerieModal(): void {
    this.invoiceSerieDataModal.type = "ADD";
    this.invoiceSerieDataModal.title = "Agregar Serie";
    this.invoiceSerieDataModal.invoiceSerie = new InvoiceSerie();
    this.invoiceSerieModal.show();
  }

  // abrir modal serie facturación modo edición.
  public editInvoiceSerieModal(id: string): void {
    this.invoiceSerieDataModal.type = "EDIT";
    this.invoiceSerieDataModal.title = "Editar Serie";
    this.invoiceSerieService.show(id)
      .subscribe(result => {
        this.invoiceSerieDataModal.invoiceSerie = result;
        this.invoiceSerieModal.show();
      });
  }

  // guardar datos de la serie de facturación.
  public saveChangesDetail(data: InvoiceSerieDataModal): void {
    if (data.type === "ADD") {
      this.invoiceSerieService.create(data.invoiceSerie)
        .subscribe(result => {
          this.invoiceSeries = _.concat(result, this.invoiceSeries);
          this.invoiceSerieModal.hide();
          toastSuccess("La serie de facturación ha sido registrado!");
        });
    }
    if (data.type === "EDIT") {
      this.invoiceSerieService.update(data.invoiceSerie.id, data.invoiceSerie)
        .subscribe(result => {
          this.invoiceSeries = _.map(this.invoiceSeries, item => {
            if (item.id === result.id) item = result;
            return item;
          });
          this.invoiceSerieModal.hide();
          toastSuccess("La serie de facturación ha sido actualizado!");
        });
    }
  }

  // borrar serie de facturación.
  public deleteInvoiceSerie(id: string): void {
    deleteConfirm().then(result => {
      if (result.isConfirmed) {
        this.invoiceSerieService.delete(id)
          .subscribe(result => {
            this.invoiceSeries = _.filter(this.invoiceSeries, item => item.id !== result.id);
          });
      }
    });
  }

}
