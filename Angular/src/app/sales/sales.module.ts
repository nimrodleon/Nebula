import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ReactiveFormsModule} from '@angular/forms';

import {SalesRoutingModule} from './sales-routing.module';
import {SalesListComponent} from './pages/sales-list/sales-list.component';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {GlobalModule} from '../global/global.module';
import {InvoiceNoteListComponent} from './pages/invoice-note-list/invoice-note-list.component';

@NgModule({
  declarations: [
    SalesListComponent,
    InvoiceNoteListComponent
  ],
  imports: [
    CommonModule,
    SalesRoutingModule,
    ReactiveFormsModule,
    FontAwesomeModule,
    GlobalModule
  ]
})
export class SalesModule {
}
