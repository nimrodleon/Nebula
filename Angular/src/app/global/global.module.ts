import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {RouterModule} from '@angular/router';
import {DocTypeInvoicePipe} from './pipes/doc-type-invoice.pipe';
import {NavbarComponent} from './navbar/navbar.component';
import {SidebarComponent} from './sidebar/sidebar.component';
import {FormaPagoPipe} from './pipes/forma-pago.pipe';
import {NoteTypePipe} from './pipes/note-type.pipe';

@NgModule({
  declarations: [
    NavbarComponent,
    SidebarComponent,
    DocTypeInvoicePipe,
    FormaPagoPipe,
    NoteTypePipe
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
    FormaPagoPipe,
    NoteTypePipe
  ]
})
export class GlobalModule {
}
