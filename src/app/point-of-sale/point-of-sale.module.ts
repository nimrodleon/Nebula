import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';

import {PointOfSaleRoutingModule} from './point-of-sale-routing.module';
import {GlobalModule} from '../global/global.module';
import {CashDetailComponent} from './pages/cash-detail/cash-detail.component';
import {CajaDiariaComponent} from './pages/caja-diaria/caja-diaria.component';

@NgModule({
  declarations: [
    CajaDiariaComponent,
    CashDetailComponent,
  ],
  imports: [
    CommonModule,
    PointOfSaleRoutingModule,
    GlobalModule,
    FontAwesomeModule
  ]
})
export class PointOfSaleModule {
}
