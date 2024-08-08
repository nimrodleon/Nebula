import {Component, OnInit} from "@angular/core";
import {ActivatedRoute} from "@angular/router";
import {InvoiceSaleService} from "../../services";
import {TicketDto} from "../../interfaces";
import _ from "lodash";
import {InvoiceTypePipe} from "app/common/pipes/invoice-type.pipe";
import {Catalogo6Pipe} from "app/common/pipes/catalogo6.pipe";
import {CurrencyPipe, DatePipe, DecimalPipe, NgForOf, NgIf} from "@angular/common";
import {SalesContainerComponent} from "app/common/containers/sales-container/sales-container.component";
import {QRCodeModule} from "angularx-qrcode";

@Component({
  selector: "app-invoice-ticket",
  standalone: true,
  imports: [
    InvoiceTypePipe,
    Catalogo6Pipe,
    DatePipe,
    SalesContainerComponent,
    CurrencyPipe,
    QRCodeModule,
    NgIf,
    DecimalPipe,
    NgForOf
  ],
  templateUrl: "./invoice-ticket.component.html"
})
export class InvoiceTicketComponent implements OnInit {
  qrCodeText: string = "";
  ticketDto = new TicketDto();

  constructor(
    private activatedRoute: ActivatedRoute,
    private invoiceSaleService: InvoiceSaleService) {
  }

  ngOnInit(): void {
    const id: string = this.activatedRoute.snapshot.params["id"];
    this.invoiceSaleService.getTicket(id)
      .subscribe(result => {
        this.ticketDto = result;
        const docTypeUser = result.invoiceSale.cliente.tipoDoc;
        const numDoc = `${result.invoiceSale.serie}-${result.invoiceSale.correlativo}`;
        this.qrCodeText = `6|${"result.configuration.ruc"}|${docTypeUser}|${result.invoiceSale.cliente.numDoc}|${result.invoiceSale.tipoDoc}|${numDoc}|${result.invoiceSale.fechaEmision}|${result.invoiceSale.totalImpuestos.toFixed(2)}|${result.invoiceSale.mtoImpVenta.toFixed(2)}|${result.invoiceSale.billingResponse.hash}`;
      });
  }

  public getMtoIgvTotal(): number {
    return _.sumBy(this.ticketDto.invoiceSaleDetails, item => item.igv);
  }

  public print(): void {
    window.print();
  }

}
