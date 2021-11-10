import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {InvoiceComponent} from './pages/invoice/invoice.component';

const routes: Routes = [{
  path: '',
  children: [
    {path: ':type', component: InvoiceComponent},
    {path: ':type/:id', component: InvoiceComponent},
    {path: '**', redirectTo: ''}
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InvoiceRoutingModule {
}
