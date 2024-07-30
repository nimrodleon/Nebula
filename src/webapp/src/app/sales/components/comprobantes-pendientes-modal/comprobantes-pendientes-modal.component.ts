import {Component, EventEmitter, OnInit, Output} from "@angular/core";
import {faChevronCircleRight, faSearch} from "@fortawesome/free-solid-svg-icons";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import _ from "lodash";
import {GetInvoiceTypePipe} from "app/common/pipes/get-invoice-type.pipe";
import {CurrencyPipe, DatePipe, NgForOf, NgIf, NgStyle} from "@angular/common";
import {RouterLink} from "@angular/router";
import {confirmTask} from "app/common/interfaces";
import {CreditNoteService, InvoiceSaleService} from "../../services";
import {ComprobantesPendientes} from "../../interfaces";

declare const bootstrap: any;

@Component({
  selector: "app-comprobantes-pendientes-modal",
  standalone: true,
  imports: [
    GetInvoiceTypePipe,
    NgForOf,
    RouterLink,
    NgIf,
    DatePipe,
    CurrencyPipe,
    NgStyle,
    FaIconComponent
  ],
  templateUrl: "./comprobantes-pendientes-modal.component.html"
})
export class ComprobantesPendientesModalComponent implements OnInit {
  faSearch = faSearch;
  faChevronCircleRight = faChevronCircleRight;
  comprobantesPendientes = new Array<ComprobantesPendientes>();
  progressModal: any;
  titleProgressModal: string = "";
  valueProgressBar: number = 0;
  @Output()
  responseData: EventEmitter<string> = new EventEmitter<string>();

  constructor(
    private invoiceSaleService: InvoiceSaleService,
    private creditNoteService: CreditNoteService) {
  }

  ngOnInit(): void {
    const myModal: any = document.querySelector("#comprobantesPendientes");
    myModal.addEventListener("shown.bs.modal", () => {
      this.invoiceSaleService.comprobantesPendientes().subscribe(result => this.comprobantesPendientes = result);
    });
    this.progressModal = new bootstrap.Modal("#progressModal");
  }

  public procesarComprobante(item: ComprobantesPendientes): void {
    this.valueProgressBar = 0;
    confirmTask().then(result => {
      if (result.isConfirmed) {
        this.progressModal.show();
        this.titleProgressModal = "Generando XML";
        this.valueProgressBar = 25;
        if (item.tipoDoc === "07") {
          this.creditNoteService.reenviar(item.comprobanteId)
            .subscribe(result => {
              this.valueProgressBar += 25;
              if (result.billingResponse.success) {
                this.comprobantesPendientes = _.filter(this.comprobantesPendientes, (item: ComprobantesPendientes) => item.comprobanteId !== result.creditNote.id);
                this.valueProgressBar += 50;
                this.titleProgressModal = result.billingResponse.cdrDescription;
              }
            });
        }
        if (item.tipoDoc !== "07") {
          this.invoiceSaleService.reenviar(item.comprobanteId)
            .subscribe(result => {
              this.valueProgressBar += 25;
              if (result.data.success) {
                this.comprobantesPendientes = _.filter(this.comprobantesPendientes, (item: ComprobantesPendientes) => item.comprobanteId !== result.invoiceId);
                this.valueProgressBar += 50;
                this.titleProgressModal = result.data.cdrDescription;
                this.responseData.emit(result.invoiceId);
              }
            });
        }
      }
    });
  }

}
