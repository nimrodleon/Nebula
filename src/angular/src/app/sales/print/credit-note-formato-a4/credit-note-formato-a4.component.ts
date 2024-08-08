import {Component, OnInit} from "@angular/core";
import {ActivatedRoute} from "@angular/router";
import {CurrencyPipe, NgForOf} from "@angular/common";
import {QRCodeModule} from "angularx-qrcode";
import _ from "lodash";
import {SalesContainerComponent} from "app/common/containers/sales-container/sales-container.component";
import {Catalogo6Pipe} from "app/common/pipes/catalogo6.pipe";
import {TipoMonedaPipe} from "app/common/pipes/tipo-moneda.pipe";
import {GetInvoiceTypePipe} from "app/common/pipes/get-invoice-type.pipe";
import {CreditNoteService} from "../../services";
import {PrintCreditNoteDto} from "../../interfaces";

@Component({
  selector: "app-credit-note-formato-a4",
  standalone: true,
  imports: [
    SalesContainerComponent,
    Catalogo6Pipe,
    TipoMonedaPipe,
    GetInvoiceTypePipe,
    CurrencyPipe,
    QRCodeModule,
    NgForOf
  ],
  templateUrl: "./credit-note-formato-a4.component.html"
})
export class CreditNoteFormatoA4Component implements OnInit {
  qrCodeText: string = "";
  printCreditNoteDto = new PrintCreditNoteDto();

  constructor(
    private activatedRoute: ActivatedRoute,
    private creditNoteService: CreditNoteService) {
  }

  ngOnInit(): void {
    const id: string = this.activatedRoute.snapshot.params["id"];
    this.creditNoteService.getPrintCreditNoteDto(id).subscribe(result => {
      this.printCreditNoteDto = result;
      const docTypeUser = result.creditNote.cliente.tipoDoc;
      const numDoc = `${result.creditNote.serie}-${result.creditNote.correlativo}`;
      this.qrCodeText = `6|${result.company.ruc}|${docTypeUser}|${result.creditNote.cliente.numDoc}|07|${numDoc}|${result.creditNote.fechaEmision}|${result.creditNote.totalImpuestos.toFixed(2)}|${result.creditNote.mtoImpVenta.toFixed(2)}|${result.creditNote.billingResponse.hash}`;
    });
  }

  public getMtoIgvTotal(): number {
    return _.sumBy(this.printCreditNoteDto.creditNoteDetails, item => item.igv);
  }

  public print(): void {
    window.print();
  }

}
