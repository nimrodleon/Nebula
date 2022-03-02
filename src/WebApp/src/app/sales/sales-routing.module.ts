import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {SalesListComponent} from './pages/sales-list/sales-list.component';
import {InvoiceNoteListComponent} from './pages/invoice-note-list/invoice-note-list.component';
import {AccountReceivableListComponent} from './pages/account-receivable-list/account-receivable-list.component';

const routes: Routes = [{
  path: '',
  children: [
    {path: '', component: SalesListComponent},
    {path: 'invoice-notes', component: InvoiceNoteListComponent},
    {path: 'account-receivable', component: AccountReceivableListComponent},
    {path: '**', redirectTo: ''}
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SalesRoutingModule {
}
