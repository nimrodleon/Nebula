import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {SalesListComponent} from './pages/sales-list/sales-list.component';

const routes: Routes = [{
  path: '',
  children: [
    {path: '', component: SalesListComponent},
    {path: '**', redirectTo: ''}
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SalesRoutingModule {
}
