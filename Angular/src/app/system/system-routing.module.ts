import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CompanyComponent} from './pages/company/company.component';
import {WarehouseListComponent} from './pages/warehouse-list/warehouse-list.component';

const routes: Routes = [{
  path: '',
  children: [
    {path: '', redirectTo: 'company'},
    {path: 'company', component: CompanyComponent},
    {path: 'warehouse', component: WarehouseListComponent},
    {path: '**', redirectTo: 'company'}
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SystemRoutingModule {
}
