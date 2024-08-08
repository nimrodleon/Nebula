import { Routes } from "@angular/router";
import { UserListComponent } from "./pages/user-list/user-list.component";
import { UserEmpresasComponent } from "./pages/user-empresas/user-empresas.component";
import { UserSuscripcionesComponent } from "./pages/user-suscripciones/user-suscripciones.component";

export const routes: Routes = [
  { path: "", component: UserListComponent },
  { path: ":userId/empresas", component: UserEmpresasComponent },
  { path: ":userId/suscripciones", component: UserSuscripcionesComponent },
];
