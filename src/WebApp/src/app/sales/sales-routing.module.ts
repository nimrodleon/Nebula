import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {InvoiceSaleDetailComponent} from './pages/invoice-sale-detail/invoice-sale-detail.component';

const routes: Routes = [{
  path: '',
  children: [
    {path: 'invoice-sale-detail/:id', component: InvoiceSaleDetailComponent},
    {path: '**', redirectTo: 'invoice-sale-detail/:id'}
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SalesRoutingModule {
}
