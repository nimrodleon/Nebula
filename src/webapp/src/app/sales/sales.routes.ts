import {Routes} from "@angular/router";
import {SalesListComponent} from "./pages/sales-list/sales-list.component";
import {SalesFormComponent} from "./pages/sales-form/sales-form.component";
import {InvoiceSaleDetailComponent} from "./pages/invoice-sale-detail/invoice-sale-detail.component";
import {InvoiceFormatoA4Component} from "./print/invoice-formato-a4/invoice-formato-a4.component";
import {CreditNoteFormatoA4Component} from "./print/credit-note-formato-a4/credit-note-formato-a4.component";
import {InvoiceTicketComponent} from "./print/invoice-ticket/invoice-ticket.component";

export const routes: Routes = [
  {path: "", component: SalesListComponent},
  {path: "form", component: SalesFormComponent},
  {path: "invoice-sale-detail/:id", component: InvoiceSaleDetailComponent},
  {path: "invoice-formato-a4/:id", component: InvoiceFormatoA4Component},
  {path: "credit-note-formato-a4/:id", component: CreditNoteFormatoA4Component},
  {path: "invoice-ticket/:id", component: InvoiceTicketComponent},
  {path: "**", redirectTo: ""}
];
