import {Routes} from "@angular/router";
import {ProductListComponent} from "./pages/product-list/product-list.component";
import {ProductDetailStockComponent} from "./pages/product-detail-stock/product-detail-stock.component";
import {CategoryListComponent} from "./pages/category-list/category-list.component";

export const routes: Routes = [
  {path: "", component: ProductListComponent},
  {path: "detail/:id/stock", component: ProductDetailStockComponent},
  {path: "category", component: CategoryListComponent},
  {path: "**", redirectTo: ""}
];
