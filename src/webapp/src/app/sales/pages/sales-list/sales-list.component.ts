import {Component, OnInit} from "@angular/core";
import {faFilter, faList, faPlus, faSearch} from "@fortawesome/free-solid-svg-icons";
import {ActivatedRoute, Router, RouterLink} from "@angular/router";
import {FormBuilder, FormGroup, ReactiveFormsModule} from "@angular/forms";
import {InvoiceSaleService} from "../../services";
import {InvoiceSale} from "../../interfaces";
import {environment} from "environments/environment";
import {
  confirmTask,
  PaginationResult,
  TipoComprobanteDataModal
} from "app/common/interfaces";
import {InvoiceSerieService} from "app/account/company/services";
import {UserDataService} from "app/common/user-data.service";
import {catchError} from "rxjs/operators";
import moment from "moment";
import _ from "lodash";
import {SalesContainerComponent} from "app/common/containers/sales-container/sales-container.component";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {CurrencyPipe, DatePipe, NgClass, NgForOf} from "@angular/common";
import {InvoiceStatusPipe} from "app/common/pipes/invoice-status.pipe";
import {LoaderComponent} from "app/common/loader/loader.component";
import {
  ComprobantesPendientesModalComponent
} from "../../components/comprobantes-pendientes-modal/comprobantes-pendientes-modal.component";
import {
  ConsultarValidezDiariaModalComponent
} from "app/common/sales/consultar-validez-diaria-modal/consultar-validez-diaria-modal.component";

declare const bootstrap: any;

@Component({
  selector: "app-sales-list",
  standalone: true,
  imports: [
    SalesContainerComponent,
    ReactiveFormsModule,
    FaIconComponent,
    RouterLink,
    DatePipe,
    NgForOf,
    NgClass,
    CurrencyPipe,
    InvoiceStatusPipe,
    LoaderComponent,
    ComprobantesPendientesModalComponent,
    ConsultarValidezDiariaModalComponent
  ],
  templateUrl: "./sales-list.component.html"
})
export class SalesListComponent implements OnInit {
  appURL: string = environment.applicationUrl;
  faSearch = faSearch;
  faPlus = faPlus;
  faList = faList;
  faFilter = faFilter;
  invoiceSales = new PaginationResult<InvoiceSale>();
  queryForm: FormGroup = this.fb.group({
    year: [moment().format("YYYY")],
    month: [moment().format("MM")],
    query: "",
  });
  comprobantesPendientesModal: any;
  consultarValidezDiariaModal: any;
  tipoComprobanteDataModal: TipoComprobanteDataModal = new TipoComprobanteDataModal();
  loading: boolean = false;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private userDataService: UserDataService,
    private invoiceSaleService: InvoiceSaleService,
    private invoiceSerieService: InvoiceSerieService) {
  }

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      const page = params["page"];
      this.cargarComprobantes(page);
    });
    this.comprobantesPendientesModal = new bootstrap.Modal("#comprobantesPendientes");
    this.consultarValidezDiariaModal = new bootstrap.Modal("#consultarValidezDiaria");
    this.invoiceSerieService.index().subscribe(result =>
      this.tipoComprobanteDataModal.invoiceSeries = result);
  }

  public get companyName(): string {
    return this.userDataService.localName;
  }

  public descargarRegistroDeVentas(event: Event): void {
    event.preventDefault();
    confirmTask().then(result => {
      if (result.isConfirmed) {
        this.loading = true;
        const year = this.queryForm.get("year")?.value;
        const month = this.queryForm.get("month")?.value;
        this.invoiceSaleService.descargarRegistroVentas(year, month)
          .pipe(catchError(err => {
            this.loading = false;
            throw err;
          })).subscribe(data => {
          const blob: Blob = new Blob([data], {type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"});
          const url: string = window.URL.createObjectURL(blob);
          const a: HTMLAnchorElement = document.createElement("a");
          a.href = url;
          a.download = "datos.xlsx"; // Cambia el nombre del archivo según tu necesidad
          document.body.appendChild(a);
          a.click();
          window.URL.revokeObjectURL(url);
          this.loading = false;
        });
      }
    });
  }

  // public descargarRegistroDeVentasF141(event: Event): void {
  //   event.preventDefault();
  //   confirmTask().then(result => {
  //     if (result.isConfirmed) {
  //       const year = this.queryForm.get("year")?.value;
  //       const month = this.queryForm.get("month")?.value;
  //       window.open(`${this.appURL}InvoiceSale/ExcelRegistroVentasF141?Year=${year}&Month=${month}`, "_blank");
  //     }
  //   });
  // }

  // public abrirModalConsultarValidezDiaria(event: Event): void {
  //   event.preventDefault();
  //   this.consultarValidezDiariaModal.show();
  // }

  public redirectConsultarValidezDiaria(fecha: string): void {
    this.consultarValidezDiariaModal.hide();
    window.open(`${this.appURL}InvoiceSale/ConsultarValidez?type=DIA&date=${fecha}`, "_blank");
  }

  // public descargarConsultaValidezMensual(event: Event): void {
  //   event.preventDefault();
  //   const year = this.queryForm.get("year")?.value;
  //   const month = this.queryForm.get("month")?.value;
  //   window.open(`${this.appURL}InvoiceSale/ConsultarValidez?type=MENSUAL&month=${month}&year=${year}`, "_blank");
  // }

  public cargarComprobantes(page: number = 1): void {
    const year = this.queryForm.get("year")?.value;
    const month = this.queryForm.get("month")?.value;
    const query = this.queryForm.get("query")?.value;
    this.invoiceSaleService.index(year, month, query, page)
      .subscribe(result => this.invoiceSales = result);
  }

  public nuevoComprobante(): void {
    this.router.navigate(["/", "sales", "form"], {
      queryParams: {
        origin: 0,
        cid: "NaN"
      }
    }).then(() => console.log("nuevoComprobante"));
  }

  public showComprobantesPendientesModal(event: Event): void {
    event.preventDefault();
    this.comprobantesPendientesModal.show();
  }

  public refreshTableList(invoiceSaleId: string): void {
    const item = _.find(this.invoiceSales.data, item => item.id === invoiceSaleId);
    if (item !== undefined) {
      this.invoiceSaleService.show(item.id).subscribe(result => {
        this.invoiceSales.data = _.map(this.invoiceSales.data, item => {
          if (item.id === result.invoiceSale.id) item = result.invoiceSale;
          return item;
        });
      });
    }
  }

}
