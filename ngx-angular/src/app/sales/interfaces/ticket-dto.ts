import {InvoiceSale, InvoiceSaleDetail} from "./invoice-sale";
import {Company} from "../../account/interfaces";

export class TicketDto {
  company: Company = new Company();
  invoiceSale: InvoiceSale = new InvoiceSale();
  invoiceSaleDetails: InvoiceSaleDetail[] = [];
}
