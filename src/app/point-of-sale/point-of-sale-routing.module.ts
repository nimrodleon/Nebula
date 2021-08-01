import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CajaDiariaComponent} from './pages/caja-diaria/caja-diaria.component';

const routes: Routes = [{
  path: '',
  children: [
    {path: '', component: CajaDiariaComponent},
    {path: '**', redirectTo: ''}
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PointOfSaleRoutingModule {
}
