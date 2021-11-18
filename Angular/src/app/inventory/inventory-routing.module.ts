import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {EntryNoteListComponent} from './pages/entry-note-list/entry-note-list.component';
import {ExitNoteListComponent} from './pages/exit-note-list/exit-note-list.component';
import {TransferListComponent} from './pages/transfer-list/transfer-list.component';
import {HistoryListComponent} from './pages/history-list/history-list.component';
import {NoteFormComponent} from './pages/note-form/note-form.component';
import {TransferFormComponent} from './pages/transfer-form/transfer-form.component';

const routes: Routes = [{
  path: '',
  children: [
    {path: '', component: HistoryListComponent},
    {path: 'entry-note', component: EntryNoteListComponent},
    {path: 'exit-note', component: ExitNoteListComponent},
    {path: 'note-form/:type', component: NoteFormComponent},
    {path: 'transfer', component: TransferListComponent},
    {path: 'transfer/form', component: TransferFormComponent},
    {path: '**', redirectTo: ''}
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InventoryRoutingModule {
}
