import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {ShoppingRoutingModule} from './shopping-routing.module';
import {ShoppingListComponent} from './pages/shopping-list/shopping-list.component';
import {GlobalModule} from '../global/global.module';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';


@NgModule({
  declarations: [
    ShoppingListComponent
  ],
  imports: [
    CommonModule,
    ShoppingRoutingModule,
    GlobalModule,
    FontAwesomeModule
  ]
})
export class ShoppingModule {
}