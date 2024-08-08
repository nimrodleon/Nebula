import {Routes} from "@angular/router";
import {PurchaseListComponent} from "./pages/purchase-list/purchase-list.component";
import {AddPurchaseComponent} from "./pages/add-purchase/add-purchase.component";
import {EditPurchaseComponent} from "./pages/edit-purchase/edit-purchase.component";

export const routes: Routes = [
  {path: "", component: PurchaseListComponent},
  {path: "add", component: AddPurchaseComponent},
  {path: "edit/:id", component: EditPurchaseComponent},
  {path: "**", redirectTo: ""}
];
