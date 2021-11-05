import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ReactiveFormsModule} from '@angular/forms';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';

import {CashierRoutingModule} from './cashier-routing.module';
import {GlobalModule} from '../global/global.module';
import {ProductsModule} from '../products/products.module';
import {ContactModule} from '../contact/contact.module';
import {CashDetailComponent} from './pages/cash-detail/cash-detail.component';
import {CajaDiariaComponent} from './pages/caja-diaria/caja-diaria.component';
import {CashInOutModalComponent} from './components/cash-in-out-modal/cash-in-out-modal.component';
import {TerminalComponent} from './pages/terminal/terminal.component';
import {CobrarModalComponent} from './components/cobrar-modal/cobrar-modal.component';
import {CerrarCajaComponent} from './components/cerrar-caja/cerrar-caja.component';
import {CuotaModalComponent} from './components/cuota-modal/cuota-modal.component';
import {InvoiceComponent} from './pages/invoice/invoice.component';

@NgModule({
  declarations: [
    CajaDiariaComponent,
    CashDetailComponent,
    CashInOutModalComponent,
    TerminalComponent,
    CobrarModalComponent,
    CerrarCajaComponent,
    CuotaModalComponent,
    InvoiceComponent,
  ],
  imports: [
    CommonModule,
    CashierRoutingModule,
    ReactiveFormsModule,
    GlobalModule,
    FontAwesomeModule,
    ProductsModule,
    ContactModule
  ]
})
export class CashierModule {
}
