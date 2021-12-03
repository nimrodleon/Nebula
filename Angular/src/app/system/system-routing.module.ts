import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CompanyComponent} from './pages/company/company.component';
import {WarehouseListComponent} from './pages/warehouse-list/warehouse-list.component';
import {InvoiceSerieListComponent} from './pages/invoice-serie-list/invoice-serie-list.component';
import {InventoryReasonListComponent} from './pages/inventory-reason-list/inventory-reason-list.component';

const routes: Routes = [{
  path: '',
  children: [
    {path: '', redirectTo: 'company'},
    {path: 'company', component: CompanyComponent},
    {path: 'warehouse', component: WarehouseListComponent},
    {path: 'invoice-serie', component: InvoiceSerieListComponent},
    {path: 'inventory-reason', component: InventoryReasonListComponent},
    {path: '**', redirectTo: 'company'}
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SystemRoutingModule {
}
