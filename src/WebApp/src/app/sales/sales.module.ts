import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {SalesRoutingModule} from './sales-routing.module';
import {InvoiceSaleDetailComponent} from './pages/invoice-sale-detail/invoice-sale-detail.component';
import {GlobalModule} from '../global/global.module';

@NgModule({
  declarations: [
    InvoiceSaleDetailComponent
  ],
  imports: [
    CommonModule,
    SalesRoutingModule,
    FontAwesomeModule,
    GlobalModule
  ]
})
export class SalesModule {
}
