import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {ReactiveFormsModule} from '@angular/forms';

import {InvoiceRoutingModule} from './invoice-routing.module';
import {InvoiceComponent} from './pages/invoice/invoice.component';
import {GlobalModule} from '../global/global.module';
import {ItemComprobanteComponent} from './components/item-comprobante/item-comprobante.component';
import {ProductsModule} from '../products/products.module';


@NgModule({
  declarations: [
    InvoiceComponent,
    ItemComprobanteComponent
  ],
  imports: [
    CommonModule,
    InvoiceRoutingModule,
    ReactiveFormsModule,
    FontAwesomeModule,
    GlobalModule,
    ProductsModule
  ]
})
export class InvoiceModule {
}
