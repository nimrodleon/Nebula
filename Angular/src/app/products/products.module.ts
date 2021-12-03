import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ReactiveFormsModule} from '@angular/forms';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';

import {ProductsRoutingModule} from './products-routing.module';
import {ProductListComponent} from './pages/product-list/product-list.component';
import {GlobalModule} from '../global/global.module';
import {ProductModalComponent} from './components/product-modal/product-modal.component';
import {CategoryModalComponent} from './components/category-modal/category-modal.component';
import {CategoryListComponent} from './pages/category-list/category-list.component';

@NgModule({
  declarations: [
    ProductListComponent,
    ProductModalComponent,
    CategoryModalComponent,
    CategoryListComponent
  ],
  exports: [
    ProductModalComponent
  ],
  imports: [
    CommonModule,
    ProductsRoutingModule,
    FontAwesomeModule,
    ReactiveFormsModule,
    GlobalModule
  ]
})
export class ProductsModule {
}
