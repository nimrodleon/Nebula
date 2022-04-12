import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CajaDiariaComponent} from './pages/caja-diaria/caja-diaria.component';
import {CashDetailComponent} from './pages/cash-detail/cash-detail.component';
import {TerminalComponent} from './pages/terminal/terminal.component';
import {TicketComponent} from './pages/ticket/ticket.component';
import {ProductReportComponent} from './pages/product-report/product-report.component';
import {FormaPagoReportComponent} from './pages/forma-pago-report/forma-pago-report.component';

const routes: Routes = [{
  path: '',
  children: [
    {path: '', component: CajaDiariaComponent},
    {path: 'terminal/:id', component: TerminalComponent},
    {path: 'detail/:id', component: CashDetailComponent},
    {path: 'ticket/:id', component: TicketComponent},
    {path: 'product-report/:id', component: ProductReportComponent},
    {path: 'forma-pago-report/:id', component: FormaPagoReportComponent},
    {path: '**', redirectTo: ''}
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CashierRoutingModule {
}
