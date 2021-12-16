import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {ReactiveFormsModule} from '@angular/forms';

import {InvoiceRoutingModule} from './invoice-routing.module';
import {InvoiceComponent} from './pages/invoice/invoice.component';
import {GlobalModule} from '../global/global.module';
import {ItemComprobanteComponent} from './components/item-comprobante/item-comprobante.component';
import {ProductsModule} from '../products/products.module';
import {InvoiceNoteComponent} from './pages/invoice-note/invoice-note.component';
import {InvoiceDetailComponent} from './pages/invoice-detail/invoice-detail.component';
import {CashierModule} from '../cashier/cashier.module';
import {ContactModule} from '../contact/contact.module';
import {CuotaModalComponent} from './components/cuota-modal/cuota-modal.component';

@NgModule({
  declarations: [
    InvoiceComponent,
    ItemComprobanteComponent,
    InvoiceNoteComponent,
    InvoiceDetailComponent,
    CuotaModalComponent
  ],
  imports: [
    CommonModule,
    InvoiceRoutingModule,
    ReactiveFormsModule,
    FontAwesomeModule,
    GlobalModule,
    ProductsModule,
    CashierModule,
    ContactModule
  ],
  exports: [
    ItemComprobanteComponent
  ]
})
export class InvoiceModule {
}
