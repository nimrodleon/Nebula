import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ReactiveFormsModule} from '@angular/forms';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';

import {InventoryRoutingModule} from './inventory-routing.module';
import {GlobalModule} from '../global/global.module';
import {InvoiceModule} from '../invoice/invoice.module';
import {TransferListComponent} from './pages/transfer-list/transfer-list.component';
import {NoteFormComponent} from './pages/note-form/note-form.component';
import {TransferFormComponent} from './pages/transfer-form/transfer-form.component';
import {InputNoteListComponent} from './pages/input-note-list/input-note-list.component';
import {OutputNoteListComponent} from './pages/output-note-list/output-note-list.component';

@NgModule({
  declarations: [
    InputNoteListComponent,
    OutputNoteListComponent,
    TransferListComponent,
    NoteFormComponent,
    TransferFormComponent
  ],
  imports: [
    CommonModule,
    InventoryRoutingModule,
    ReactiveFormsModule,
    FontAwesomeModule,
    GlobalModule,
    InvoiceModule
  ]
})
export class InventoryModule {
}
