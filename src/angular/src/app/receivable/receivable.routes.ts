import {Routes} from "@angular/router";
import {ReceivableListComponent} from "./pages/receivable-list/receivable-list.component";
import {PendientesPorCobrarComponent} from "./pages/pendientes-por-cobrar/pendientes-por-cobrar.component";

export const routes: Routes = [
  {path: "", component: ReceivableListComponent},
  {path: "pendientes-por-cobrar", component: PendientesPorCobrarComponent},
  {path: "**", redirectTo: ""}
];
