import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ContactListComponent} from './pages/contact-list/contact-list.component';
import {DocTypeListComponent} from './pages/doc-type-list/doc-type-list.component';

const routes: Routes = [{
  path: '',
  children: [
    {path: '', component: ContactListComponent},
    {path: 'doc-type', component: DocTypeListComponent},
    {path: '**', redirectTo: ''}
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ContactRoutingModule {
}
