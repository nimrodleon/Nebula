import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {TransferListComponent} from './pages/transfer-list/transfer-list.component';
import {NoteFormComponent} from './pages/note-form/note-form.component';
import {TransferFormComponent} from './pages/transfer-form/transfer-form.component';
import {InputNoteListComponent} from './pages/input-note-list/input-note-list.component';
import {OutputNoteListComponent} from './pages/output-note-list/output-note-list.component';

const routes: Routes = [{
  path: '',
  children: [
    {path: 'input-note', component: InputNoteListComponent},
    {path: 'output-note', component: OutputNoteListComponent},
    {path: 'note-form/:type', component: NoteFormComponent},
    {path: 'note-form/:type/:id', component: NoteFormComponent},
    {path: 'transfer', component: TransferListComponent},
    {path: 'transfer/form', component: TransferFormComponent},
    {path: 'transfer/form/:id', component: TransferFormComponent},
    {path: '**', redirectTo: 'transfer'}
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InventoryRoutingModule {
}
