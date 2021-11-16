import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {InvoiceComponent} from './pages/invoice/invoice.component';
import {InvoiceNoteComponent} from './pages/invoice-note/invoice-note.component';

const routes: Routes = [{
  path: '',
  children: [
    {path: 'note', component: InvoiceNoteComponent},
    {path: ':type', component: InvoiceComponent},
    {path: ':type/:id', component: InvoiceComponent},
    {path: '**', redirectTo: ''}
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InvoiceRoutingModule {
}
