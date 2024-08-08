import { Routes } from "@angular/router";
import { LoginComponent } from "./login/login.component";
import { companyGuard } from "./company.guard";
import { authGuard } from "./auth.guard";

export const routes: Routes = [
  { path: "login", component: LoginComponent },
  {
    path: "public",
    loadChildren: () => import("./public/public.routes").then((m) => m.routes)
  },
  {
    path: "account",
    loadChildren: () => import("./account/account.routes").then((m) => m.routes),
    canActivate: [authGuard]
  },
  {
    path: ":companyId",
    children: [
      {
        path: "contacts",
        loadChildren: () => import("./contact/contact.routes").then((m) => m.routes)
      },
      {
        path: "products",
        loadChildren: () => import("./products/products.routes").then((m) => m.routes)
      },
      {
        path: "inventories",
        loadChildren: () => import("./inventory/inventory.routes").then((m) => m.routes)
      },
      {
        path: "sales",
        loadChildren: () => import("./sales/sales.routes").then((m) => m.routes)
      },
      {
        path: "receivables",
        loadChildren: () => import("./receivable/receivable.routes").then((m) => m.routes)
      },
      {
        path: "taller-reparaciones",
        loadChildren: () => import("./taller/taller.routes").then((m) => m.routes)
      },
      {
        path: "cashier",
        loadChildren: () => import("./cashier/cashier.routes").then((m) => m.routes)
      },
      // {
      //   path: "purchases",
      //   loadChildren: () => import("./purchases/purchases.routes").then((m) => m.routes),
      //   canActivate: [authGuard]
      // },
    ],
    canActivate: [authGuard, companyGuard]
  },
  { path: "", redirectTo: "/account", pathMatch: "full" }
];
