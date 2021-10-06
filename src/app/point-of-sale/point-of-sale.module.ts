import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';

import {PointOfSaleRoutingModule} from './point-of-sale-routing.module';
import {GlobalModule} from '../global/global.module';
import {CashDetailComponent} from './pages/cash-detail/cash-detail.component';
import {CajaDiariaComponent} from './pages/caja-diaria/caja-diaria.component';
import {CashInOutModalComponent} from './components/cash-in-out-modal/cash-in-out-modal.component';
import {TerminalComponent} from './pages/terminal/terminal.component';

@NgModule({
  declarations: [
    CajaDiariaComponent,
    CashDetailComponent,
    CashInOutModalComponent,
    TerminalComponent,
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
