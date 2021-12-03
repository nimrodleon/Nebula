import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ProductListComponent} from './pages/product-list/product-list.component';
import {CategoryListComponent} from './pages/category-list/category-list.component';

const routes: Routes = [{
  path: '',
  children: [
    {path: '', component: ProductListComponent},
    {path: 'category', component: CategoryListComponent},
    {path: '**', redirectTo: ''}
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProductsRoutingModule {
}
