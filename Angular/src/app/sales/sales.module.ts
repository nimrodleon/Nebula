import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ReactiveFormsModule} from '@angular/forms';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {SalesRoutingModule} from './sales-routing.module';
import {GlobalModule} from '../global/global.module';
import {InvoiceModule} from '../invoice/invoice.module';
import {SalesListComponent} from './pages/sales-list/sales-list.component';
import {InvoiceNoteListComponent} from './pages/invoice-note-list/invoice-note-list.component';
import {AccountReceivableListComponent} from './pages/account-receivable-list/account-receivable-list.component';

@NgModule({
  declarations: [
    SalesListComponent,
    InvoiceNoteListComponent,
    AccountReceivableListComponent
  ],
  imports: [
    CommonModule,
    SalesRoutingModule,
    ReactiveFormsModule,
    FontAwesomeModule,
    GlobalModule,
    InvoiceModule
  ]
})
export class SalesModule {
}
