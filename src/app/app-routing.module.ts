import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LoginComponent} from './login/login.component';

const routes: Routes = [
  {path: 'login', component: LoginComponent},
  {
    path: 'products',
    loadChildren: () => import('./products/products.module').then(m => m.ProductsModule)
  },
  {
    path: 'point-of-sale',
    loadChildren: () => import('./point-of-sale/point-of-sale.module').then(m => m.PointOfSaleModule)
  },
  {
    path: 'shopping',
    loadChildren: () => import('./shopping/shopping.module').then(m => m.ShoppingModule)
  },
  {
    path: 'sales',
    loadChildren: () => import('./sales/sales.module').then(m => m.SalesModule)
  },
  {
    path: 'inventory',
    loadChildren: () => import('./inventory/inventory.module').then(m => m.InventoryModule)
  },
  {
    path: 'contacts',
    loadChildren: () => import('./contact/contact.module').then(m => m.ContactModule)
  },
  {path: '', redirectTo: '/products', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
