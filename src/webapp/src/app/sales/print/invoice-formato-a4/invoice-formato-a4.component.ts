import {Component, OnInit} from "@angular/core";
import {TicketDto} from "../../interfaces";
import {ActivatedRoute} from "@angular/router";
import {InvoiceSaleService} from "../../services";
import _ from "lodash";
import {InvoiceTypePipe} from "app/common/pipes/invoice-type.pipe";
import {CurrencyPipe, DatePipe, NgForOf, NgIf} from "@angular/common";
import {Catalogo6Pipe} from "app/common/pipes/catalogo6.pipe";
import {TipoMonedaPipe} from "app/common/pipes/tipo-moneda.pipe";
import {QRCodeModule} from "angularx-qrcode";
import {SalesContainerComponent} from "app/common/containers/sales-container/sales-container.component";

@Component({
  selector: "app-invoice-formato-a4",
  standalone: true,
  imports: [
    InvoiceTypePipe,
    DatePipe,
    Catalogo6Pipe,
    TipoMonedaPipe,
    CurrencyPipe,
    QRCodeModule,
    NgIf,
    SalesContainerComponent,
    NgForOf
  ],
  templateUrl: "./invoice-formato-a4.component.html"
})
export class InvoiceFormatoA4Component implements OnInit {
  qrCodeText: string = "";
  ticketDto = new TicketDto();

  constructor(
    private activatedRoute: ActivatedRoute,
    private invoiceSaleService: InvoiceSaleService) {
  }

  ngOnInit(): void {
    const id: string = this.activatedRoute.snapshot.params["id"];
    this.invoiceSaleService.getTicket(id).subscribe(result => {
      this.ticketDto = result;
      const docTypeUser = result.invoiceSale.cliente.tipoDoc;
      const docTypeComprobante = result.invoiceSale.tipoDoc;
      const numDoc = `${result.invoiceSale.serie}-${result.invoiceSale.correlativo}`;
      this.qrCodeText = `6|${result.company.ruc}|${docTypeUser}|${result.invoiceSale.cliente.numDoc}|${docTypeComprobante}|${numDoc}|${result.invoiceSale.fechaEmision}|${result.invoiceSale.totalImpuestos.toFixed(2)}|${result.invoiceSale.mtoImpVenta.toFixed(2)}|${result.invoiceSale.billingResponse.hash}`;
    });
  }

  public getMtoIgvTotal(): number {
    return _.sumBy(this.ticketDto.invoiceSaleDetails, item => item.igv);
  }

  public print(): void {
    window.print();
  }

}
