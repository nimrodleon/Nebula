import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';

import {InventoryRoutingModule} from './inventory-routing.module';
import {GlobalModule} from '../global/global.module';
import {EntryNoteListComponent} from './pages/entry-note-list/entry-note-list.component';
import {ExitNoteListComponent} from './pages/exit-note-list/exit-note-list.component';
import {TransferListComponent} from './pages/transfer-list/transfer-list.component';
import {HistoryListComponent} from './pages/history-list/history-list.component';
import {NoteFormComponent} from './pages/note-form/note-form.component';
import {TransferFormComponent} from './pages/transfer-form/transfer-form.component';

@NgModule({
  declarations: [
    EntryNoteListComponent,
    ExitNoteListComponent,
    TransferListComponent,
    HistoryListComponent,
    NoteFormComponent,
    TransferFormComponent
  ],
  imports: [
    CommonModule,
    InventoryRoutingModule,
    FontAwesomeModule,
    GlobalModule
  ]
})
export class InventoryModule {
}
