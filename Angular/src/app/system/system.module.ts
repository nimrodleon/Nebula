import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';

import {SystemRoutingModule} from './system-routing.module';
import {CompanyComponent} from './pages/company/company.component';
import {GlobalModule} from '../global/global.module';
import {WarehouseListComponent} from './pages/warehouse-list/warehouse-list.component';
import {WarehouseModalComponent} from './components/warehouse-modal/warehouse-modal.component';


@NgModule({
  declarations: [
    CompanyComponent,
    WarehouseListComponent,
    WarehouseModalComponent
  ],
  imports: [
    CommonModule,
    SystemRoutingModule,
    GlobalModule,
    FontAwesomeModule
  ]
})
export class SystemModule {
}
