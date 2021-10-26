import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {SalesRoutingModule} from './sales-routing.module';
import {SalesListComponent} from './pages/sales-list/sales-list.component';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {GlobalModule} from '../global/global.module';


@NgModule({
  declarations: [
    SalesListComponent
  ],
  imports: [
    CommonModule,
    SalesRoutingModule,
    FontAwesomeModule,
    GlobalModule
  ]
})
export class SalesModule {
}