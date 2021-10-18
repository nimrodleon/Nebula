import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {ProductsRoutingModule} from './products-routing.module';
import {ProductListComponent} from './pages/product-list/product-list.component';
import {GlobalModule} from '../global/global.module';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import { ProductModalComponent } from './components/product-modal/product-modal.component';


@NgModule({
  declarations: [
    ProductListComponent,
    ProductModalComponent
  ],
  imports: [
    CommonModule,
    ProductsRoutingModule,
    GlobalModule,
    FontAwesomeModule
  ]
})
export class ProductsModule {
}
