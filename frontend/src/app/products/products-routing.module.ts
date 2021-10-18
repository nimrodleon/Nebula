import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ProductListComponent} from './pages/product-list/product-list.component';

const routes: Routes = [{
  path: '',
  children: [
    {path: '', component: ProductListComponent},
    {path: '**', redirectTo: ''}
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProductsRoutingModule {
}
