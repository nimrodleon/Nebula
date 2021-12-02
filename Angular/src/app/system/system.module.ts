import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';

import {SystemRoutingModule} from './system-routing.module';
import {CompanyComponent} from './pages/company/company.component';
import {GlobalModule} from '../global/global.module';
import {WarehouseListComponent} from './pages/warehouse-list/warehouse-list.component';


@NgModule({
  declarations: [
    CompanyComponent,
    WarehouseListComponent
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
