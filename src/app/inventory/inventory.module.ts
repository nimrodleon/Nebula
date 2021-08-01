import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {InventoryRoutingModule} from './inventory-routing.module';
import {InventoryListComponent} from './pages/inventory-list/inventory-list.component';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {GlobalModule} from '../global/global.module';


@NgModule({
  declarations: [
    InventoryListComponent
  ],
  imports: [
    CommonModule,
    InventoryRoutingModule,
    FontAwesomeModule,
    GlobalModule
  ]
})
export class InventoryModule {
}
