import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {SalesRoutingModule} from './sales-routing.module';
import {InvoiceSaleDetailComponent} from './pages/invoice-sale-detail/invoice-sale-detail.component';

@NgModule({
  declarations: [
    InvoiceSaleDetailComponent
  ],
  imports: [
    CommonModule,
    SalesRoutingModule
  ]
})
export class SalesModule {
}
