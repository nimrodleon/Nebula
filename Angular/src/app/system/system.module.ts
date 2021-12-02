import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {SystemRoutingModule} from './system-routing.module';
import {CompanyComponent} from './pages/company/company.component';
import {GlobalModule} from '../global/global.module';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';


@NgModule({
  declarations: [
    CompanyComponent
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
