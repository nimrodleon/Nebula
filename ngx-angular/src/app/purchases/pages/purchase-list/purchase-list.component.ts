import {Component, OnInit} from "@angular/core";
import {FormBuilder, FormGroup, ReactiveFormsModule} from "@angular/forms";
import {faCartShopping, faEdit, faList, faPlus, faSearch, faTrashAlt} from "@fortawesome/free-solid-svg-icons";
import {PurchaseInvoice} from "../../interfaces/purchase-invoice";
import {PurchaseInvoiceService} from "../../services";
import {EnumIdModal, confirmTask, deleteConfirm} from "app/common/interfaces";
import {environment} from "environments/environment";
import moment from "moment";
import _ from "lodash";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {RouterLink} from "@angular/router";
import {CurrencyPipe, DatePipe, NgForOf} from "@angular/common";
import {
  ConsultarValidezDiariaModalComponent
} from "app/common/sales/consultar-validez-diaria-modal/consultar-validez-diaria-modal.component";

declare const bootstrap: any;

@Component({
  selector: "app-purchase-list",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FaIconComponent,
    RouterLink,
    CurrencyPipe,
    DatePipe,
    ConsultarValidezDiariaModalComponent,
    NgForOf
  ],
  templateUrl: "./purchase-list.component.html"
})
export class PurchaseListComponent implements OnInit {
  appURL: string = environment.applicationUrl;
  protected readonly faSearch = faSearch;
  protected readonly faPlus = faPlus;
  protected readonly faList = faList;
  protected readonly faCartShopping = faCartShopping;
  protected readonly faEdit = faEdit;
  protected readonly faTrashAlt = faTrashAlt;
  consultarValidezDiariaModal: any;
  purchaseInvoices = new Array<PurchaseInvoice>();
  queryForm: FormGroup = this.fb.group({
    year: [moment().format("YYYY")],
    month: [moment().format("MM")],
  });

  constructor(
    private fb: FormBuilder,
    private purchaseInvoiceService: PurchaseInvoiceService) {
  }

  ngOnInit(): void {
    this.cargarComprobantes();
    this.consultarValidezDiariaModal = new bootstrap.Modal(EnumIdModal.CONSULTAR_VALIDEZ_DIARIA);
  }

  public cargarComprobantes(): void {
    const year = this.queryForm.get("year")?.value;
    const month = this.queryForm.get("month")?.value;
    this.purchaseInvoiceService.index(year, month).subscribe(result => this.purchaseInvoices = result);
  }

  public deleteComprobante(id: string): void {
    deleteConfirm().then(result => {
      if (result.isConfirmed) {
        this.purchaseInvoiceService.delete(id)
          .subscribe(result => {
            this.purchaseInvoices = _.filter(this.purchaseInvoices, item => item.id !== result.id);
          });
      }
    });
  }

  public abrirModalConsultarValidezDiaria(event: Event): void {
    event.preventDefault();
    this.consultarValidezDiariaModal.show();
  }

  public redirectConsultarValidezDiaria(fecha: string): void {
    this.consultarValidezDiariaModal.hide();
    window.open(`${this.appURL}PurchaseInvoice/ConsultarValidez?type=DIA&date=${fecha}`, "_blank");
  }

  public descargarConsultaValidezMensual(event: Event): void {
    event.preventDefault();
    const year = this.queryForm.get("year")?.value;
    const month = this.queryForm.get("month")?.value;
    window.open(`${this.appURL}PurchaseInvoice/ConsultarValidez?type=MENSUAL&month=${month}&year=${year}`, "_blank");
  }

  public descargarRegistroDeComprasF81(event: Event): void {
    event.preventDefault();
    confirmTask().then(result => {
      if (result.isConfirmed) {
        const year = this.queryForm.get("year")?.value;
        const month = this.queryForm.get("month")?.value;
        window.open(`${this.appURL}PurchaseInvoice/ExcelRegistroComprasF81?Year=${year}&Month=${month}`, "_blank");
      }
    });
  }

}
