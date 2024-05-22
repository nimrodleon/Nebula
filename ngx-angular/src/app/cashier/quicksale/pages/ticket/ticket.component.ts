import {Component, OnInit} from "@angular/core";
import {ActivatedRoute} from "@angular/router";
import {TicketDto} from "app/sales/interfaces";
import {InvoiceSaleService} from "app/sales/services";
import _ from "lodash";
import {InvoiceTypePipe} from "app/common/pipes/invoice-type.pipe";
import {CashierContainerComponent} from "app/common/containers/cashier-container/cashier-container.component";
import {Catalogo6Pipe} from "app/common/pipes/catalogo6.pipe";
import {CurrencyPipe, DatePipe, NgForOf, NgIf} from "@angular/common";
import {QRCodeModule} from "angularx-qrcode";

@Component({
  selector: "app-ticket",
  standalone: true,
  imports: [
    InvoiceTypePipe,
    CashierContainerComponent,
    Catalogo6Pipe,
    DatePipe,
    CurrencyPipe,
    NgForOf,
    QRCodeModule,
    NgIf
  ],
  templateUrl: "./ticket.component.html"
})
export class TicketComponent implements OnInit {
  qrCodeText: string = "";
  ticketDto = new TicketDto();
  companyId: string = "";

  constructor(
    private activatedRoute: ActivatedRoute,
    private invoiceSaleService: InvoiceSaleService) {
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
      const id: string = params.get("id") || "";
      this.invoiceSaleService.getTicket(id)
        .subscribe(result => {
          this.ticketDto = result;
          const docTypeUser = result.invoiceSale.cliente.tipoDoc;
          let docTypeComprobante = result.invoiceSale.tipoDoc;
          const numDoc = `${result.invoiceSale.serie}-${result.invoiceSale.correlativo}`;
          this.qrCodeText = `6|${result.company.ruc}|${docTypeUser}|${result.invoiceSale.cliente.numDoc}|${docTypeComprobante}|${numDoc}|${result.invoiceSale.fechaEmision}|${result.invoiceSale.totalImpuestos.toFixed(2)}|${result.invoiceSale.mtoImpVenta.toFixed(2)}|${result.invoiceSale.billingResponse.hash}`;
        });
    });
  }

  public getMtoIgvTotal(): number {
    return _.sumBy(this.ticketDto.invoiceSaleDetails, item => item.igv);
  }

  public print(): void {
    window.print();
  }

}
