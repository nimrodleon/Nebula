import {Routes} from "@angular/router";
import {CajaDiariaComponent} from "./pages/caja-diaria/caja-diaria.component";
import {QuickSaleComponent} from "./quicksale/pages/quick-sale/quick-sale.component";
import {CashDetailComponent} from "./pages/cash-detail/cash-detail.component";
import {TicketComponent} from "./quicksale/pages/ticket/ticket.component";
import {ProductReportComponent} from "./pages/product-report/product-report.component";

export const routes: Routes = [
  {path: "", component: CajaDiariaComponent},
  {path: "quicksale/:id", component: QuickSaleComponent},
  {path: "detail/:id", component: CashDetailComponent},
  {path: "ticket/:id", component: TicketComponent},
  {path: "product-report/:id", component: ProductReportComponent},
  {path: "**", redirectTo: ""}
];
