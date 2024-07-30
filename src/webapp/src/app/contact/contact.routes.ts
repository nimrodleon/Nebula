import {Routes} from "@angular/router";
import {ContactListComponent} from "./pages/contact-list/contact-list.component";
import {ContactDetailVentasComponent} from "./pages/contact-detail-ventas/contact-detail-ventas.component";
import {ContactDetailReceivableComponent} from "./pages/contact-detail-receivable/contact-detail-receivable.component";
import {ContactDetailDineroComponent} from "./pages/contact-detail-dinero/contact-detail-dinero.component";

export const routes: Routes = [
  {path: "", component: ContactListComponent},
  {path: "detail/:id/ventas", component: ContactDetailVentasComponent},
  {path: "detail/:id/receivable", component: ContactDetailReceivableComponent},
  {path: "detail/:id/dinero", component: ContactDetailDineroComponent},
  {path: "**", redirectTo: ""}
];
