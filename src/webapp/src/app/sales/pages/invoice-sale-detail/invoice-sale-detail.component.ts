import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, RouterLink } from "@angular/router";
import { faChevronCircleRight, faCircleXmark, faDownload, faPrint, faRotate } from "@fortawesome/free-solid-svg-icons";
import { CreditNoteService, InvoiceSaleService } from "../../services";
import { CreditNote, ResponseInvoiceSale } from "../../interfaces";
import { confirmTask } from "app/common/interfaces";
import Swal from "sweetalert2";
import _ from "lodash";
import { catchError } from "rxjs/operators";
import { InvoiceTypePipe } from "app/common/pipes/invoice-type.pipe";
import { SalesContainerComponent } from "app/common/containers/sales-container/sales-container.component";
import { FaIconComponent } from "@fortawesome/angular-fontawesome";
import { CurrencyPipe, DatePipe, NgForOf, NgIf, NgStyle } from "@angular/common";
import { LoaderComponent } from "app/common/loader/loader.component";

declare const bootstrap: any;

@Component({
  selector: "app-invoice-sale-detail",
  templateUrl: "./invoice-sale-detail.component.html",
  standalone: true,
  imports: [
    InvoiceTypePipe,
    SalesContainerComponent,
    RouterLink,
    FaIconComponent,
    DatePipe,
    CurrencyPipe,
    NgStyle,
    LoaderComponent,
    NgIf,
    NgForOf
  ],
  styleUrls: ["./invoice-sale-detail.component.scss"]
})
export class InvoiceSaleDetailComponent implements OnInit {
  faRotate = faRotate;
  faChevronCircleRight = faChevronCircleRight;
  faPrint = faPrint;
  faCircleXmark = faCircleXmark;
  faDownload = faDownload;
  creditNote: CreditNote = new CreditNote();
  responseInvoiceSale: ResponseInvoiceSale = new ResponseInvoiceSale();
  titleProgressModal: string = "";
  valueProgressBar: number = 0;
  progressModal: any;
  loading: boolean = false;

  constructor(
    private activatedRoute: ActivatedRoute,
    private invoiceSaleService: InvoiceSaleService,
    private creditNoteService: CreditNoteService) {
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      const id: string = params.get("id") || "";
      this.invoiceSaleService.show(id)
        .subscribe(result => {
          this.responseInvoiceSale = result;
          if (result.invoiceSale.anulada) {
            this.creditNoteService.show(result.invoiceSale.id)
              .subscribe(result => {
                this.creditNote = result;
              });
          }
        });
    });
    this.progressModal = new bootstrap.Modal("#progressModal");
    // establecer ancho de la página por defecto.
    const myModal: any = document.querySelector("#progressModal");
    myModal.addEventListener("hidden.bs.modal", () => {
      document.body.style.paddingRight = "0px";
    });
  }

  public anularComprobante(event: Event): void {
    event.preventDefault();
    confirmTask().then(result => {
      if (result.isConfirmed) {
        if (this.responseInvoiceSale.invoiceSale.anulada) {
          Swal.fire(
            "Oops...",
            "El comprobante ya está anulado!",
            "error"
          ).then(() => console.log(":("));
        } else {
          this.valueProgressBar = 0;
          this.titleProgressModal = "Crear Comprobante!";
          this.progressModal.show();
          const { id } = this.responseInvoiceSale.invoiceSale;
          this.invoiceSaleService.anularComprobante(id)
            .subscribe(result => {
              this.valueProgressBar = 50;
              this.titleProgressModal = "Comprobante registrado!";
              if (result.billingResponse.success) {
                this.responseInvoiceSale.invoiceSale.anulada = result.billingResponse.success;
                this.creditNote = result.creditNote;
                this.valueProgressBar += 50;
                this.titleProgressModal = result.billingResponse.cdrDescription;
              } else {
                this.valueProgressBar += 50;
                this.titleProgressModal = result.billingResponse.cdrDescription;
              }
            });
        }
      }
    });
  }

  public getMtoIgvTotal(): number {
    return _.sumBy(this.responseInvoiceSale.invoiceSaleDetails, item => item.igv);
  }

  public reenviarCreditNote(): void {
    confirmTask().then(result => {
      if (result.isConfirmed) {
        this.loading = true;
        this.creditNoteService.reenviar(this.creditNote.id)
          .pipe(catchError(err => {
            this.loading = false;
            throw err;
          })).subscribe(result => {
            this.creditNote = result.creditNote;
            this.loading = false;
          });
      }
    });
  }

  public descargarXML(event: Event): void {
    event.preventDefault();
    confirmTask().then(result => {
      if (result.isConfirmed) {
        this.loading = true;
        const invoice = this.responseInvoiceSale.invoiceSale;
        this.invoiceSaleService.getXml(invoice.id)
          .pipe(catchError(err => {
            this.loading = false;
            throw err;
          })).subscribe(data => {
            const blob: Blob = new Blob([data], { type: "application/xml" });
            const url: string = window.URL.createObjectURL(blob);
            const a: HTMLAnchorElement = document.createElement("a");
            a.href = url;
            a.download = `${invoice.serie}-${invoice.correlativo}.xml`;
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
            this.loading = false;
          });
      }
    });
  }

}
