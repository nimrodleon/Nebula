import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ConfigurationComponent} from './pages/configuration/configuration.component';
import {WarehouseListComponent} from './pages/warehouse-list/warehouse-list.component';
import {InvoiceSerieListComponent} from './pages/invoice-serie-list/invoice-serie-list.component';
import {InventoryReasonListComponent} from './pages/inventory-reason-list/inventory-reason-list.component';
import {SystemListComponent} from './pages/system-list/system-list.component';

const routes: Routes = [{
  path: '',
  children: [
    {path: '', component: SystemListComponent},
    {path: 'configuration', component: ConfigurationComponent},
    {path: 'warehouse', component: WarehouseListComponent},
    {path: 'invoice-serie', component: InvoiceSerieListComponent},
    {path: 'inventory-reason', component: InventoryReasonListComponent},
    {path: '**', redirectTo: ''}
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SystemRoutingModule {
}