import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {RouterModule} from '@angular/router';
import {DocTypeInvoicePipe} from './pipes/doc-type-invoice.pipe';
import {NavbarComponent} from './navbar/navbar.component';
import {SidebarComponent} from './sidebar/sidebar.component';
import {FormaPagoPipe} from './pipes/forma-pago.pipe';

@NgModule({
  declarations: [
    NavbarComponent,
    SidebarComponent,
    DocTypeInvoicePipe,
    FormaPagoPipe
  ],
  imports: [
    CommonModule,
    FontAwesomeModule,
    RouterModule
  ],
  exports: [
    NavbarComponent,
    SidebarComponent,
    DocTypeInvoicePipe,
    FormaPagoPipe
  ]
})
export class GlobalModule {
}
