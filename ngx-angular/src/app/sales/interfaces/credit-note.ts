import {Company} from "app/account/interfaces";
import {BaseSale, SaleDetail} from "./invoice-sale";

export class CreditNote extends BaseSale {
  invoiceSaleId: string = "";
  codMotivo: string = "";
  desMotivo: string = "";
  tipDocAfectado: string = "";
  numDocfectado: string = "";
}

export class CreditNoteDetail extends SaleDetail {
  creditNoteId: string = "";
}

export class PrintCreditNoteDto {
  company: Company = new Company();
  creditNote: CreditNote = new CreditNote();
  creditNoteDetails: CreditNoteDetail[] = [];
}
