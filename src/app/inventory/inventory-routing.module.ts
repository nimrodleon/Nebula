import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {InventoryListComponent} from './pages/inventory-list/inventory-list.component';

const routes: Routes = [{
  path: '',
  children: [
    {path: '', component: InventoryListComponent},
    {path: '**', redirectTo: ''}
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InventoryRoutingModule {
}
