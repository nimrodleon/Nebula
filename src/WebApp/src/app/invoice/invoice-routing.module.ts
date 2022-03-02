import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {InvoiceComponent} from './pages/invoice/invoice.component';
import {InvoiceNoteComponent} from './pages/invoice-note/invoice-note.component';
import {InvoiceDetailComponent} from './pages/invoice-detail/invoice-detail.component';

const routes: Routes = [{
  path: '',
  children: [
    {path: 'note/:type/:invoiceId', component: InvoiceNoteComponent},
    {path: 'note/:type/:invoiceId/:id', component: InvoiceNoteComponent},
    {path: 'detail/:type/:id', component: InvoiceDetailComponent},
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
