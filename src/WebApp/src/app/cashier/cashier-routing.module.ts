import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CajaDiariaComponent} from './pages/caja-diaria/caja-diaria.component';
import {CashDetailComponent} from './pages/cash-detail/cash-detail.component';
import {TerminalComponent} from './pages/terminal/terminal.component';

const routes: Routes = [{
  path: '',
  children: [
    {path: '', component: CajaDiariaComponent},
    {path: 'terminal/:id', component: TerminalComponent},
    {path: 'detail/:id', component: CashDetailComponent},
    {path: '**', redirectTo: ''}
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CashierRoutingModule {
}
