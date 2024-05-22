import { Routes } from "@angular/router";
import { ShortcutsComponent } from "./pages/shortcuts/shortcuts.component";
import { CompanyListComponent } from "./pages/company-list/company-list.component";
import { CompanyAddComponent } from "./pages/company-add/company-add.component";
import { CompanyEditComponent } from "./pages/company-edit/company-edit.component";
import { CertificadoComponent } from "./company/pages/certificado/certificado.component";
import { WarehouseListComponent } from "./company/pages/warehouse-list/warehouse-list.component";
import { InvoiceSerieListComponent } from "./company/pages/invoice-serie-list/invoice-serie-list.component";
import { CollaboratorListComponent } from "./company/pages/collaborator-list/collaborator-list.component";
import { ListaEmpresasComponent } from "./empresas/pages/lista-empresas/lista-empresas.component";

export const routes: Routes = [
  { path: "", component: ShortcutsComponent },
  { path: "users", loadChildren: () => import("./user/users.routes").then((m) => m.routes) },
  { path: "mycompanies", component: CompanyListComponent },
  { path: "mycompanies/add", component: CompanyAddComponent },
  { path: "mycompanies/:id/edit", component: CompanyEditComponent },
  { path: "mycompanies/:companyId/certificado", component: CertificadoComponent },
  { path: "mycompanies/:companyId/warehouses", component: WarehouseListComponent },
  { path: "mycompanies/:companyId/invoice-series", component: InvoiceSerieListComponent },
  { path: "mycompanies/:companyId/collaborators", component: CollaboratorListComponent },
  { path: "empresas", component: ListaEmpresasComponent },
  { path: "**", redirectTo: "" }
];
