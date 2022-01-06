import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ShoppingListComponent} from './pages/shopping-list/shopping-list.component';
import {InvoiceNoteListComponent} from './pages/invoice-note-list/invoice-note-list.component';

const routes: Routes = [{
  path: '',
  children: [
    {path: '', component: ShoppingListComponent},
    {path: 'invoice-notes', component: InvoiceNoteListComponent},
    {path: '**', redirectTo: ''}
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ShoppingRoutingModule {
}
