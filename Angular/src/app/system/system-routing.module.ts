import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CompanyComponent} from './pages/company/company.component';

const routes: Routes = [{
  path: '',
  children: [
    {path: '', redirectTo: 'company'},
    {path: 'company', component: CompanyComponent},
    {path: '**', redirectTo: 'company'}
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SystemRoutingModule {
}
