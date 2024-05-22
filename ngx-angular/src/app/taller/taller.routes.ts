import {Routes} from "@angular/router";
import {ListaReparacionesComponent} from "./pages/lista-reparaciones/lista-reparaciones.component";
import {OrdenReparacionAddComponent} from "./pages/orden-reparacion-add/orden-reparacion-add.component";
import {OrdenReparacionEditComponent} from "./pages/orden-reparacion-edit/orden-reparacion-edit.component";
import {RepairOrderTicketComponent} from "./pages/repair-order-ticket/repair-order-ticket.component";
import {RepairOrderMonthlyComponent} from "./report/repair-order-monthly/repair-order-monthly.component";

export const routes: Routes = [
  {path: "", component: ListaReparacionesComponent},
  {path: "orden-reparacion/add", component: OrdenReparacionAddComponent},
  {path: "orden-reparacion/edit/:id", component: OrdenReparacionEditComponent},
  {path: "ticket/:id", component: RepairOrderTicketComponent},
  {path: "monthly-report", component: RepairOrderMonthlyComponent},
  {path: "**", redirectTo: ""}
];
