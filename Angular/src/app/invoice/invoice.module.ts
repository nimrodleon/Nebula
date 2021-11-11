import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {ReactiveFormsModule} from '@angular/forms';

import {InvoiceRoutingModule} from './invoice-routing.module';
import {InvoiceComponent} from './pages/invoice/invoice.component';
import {GlobalModule} from '../global/global.module';


@NgModule({
  declarations: [
    InvoiceComponent
  ],
  imports: [
    CommonModule,
    InvoiceRoutingModule,
    ReactiveFormsModule,
    FontAwesomeModule,
    GlobalModule
  ]
})
export class InvoiceModule {
}
