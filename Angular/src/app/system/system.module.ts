import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ReactiveFormsModule} from '@angular/forms';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';

import {SystemRoutingModule} from './system-routing.module';
import {CompanyComponent} from './pages/company/company.component';
import {GlobalModule} from '../global/global.module';
import {WarehouseListComponent} from './pages/warehouse-list/warehouse-list.component';
import {WarehouseModalComponent} from './components/warehouse-modal/warehouse-modal.component';
import {InvoiceSerieListComponent} from './pages/invoice-serie-list/invoice-serie-list.component';
import {InvoiceSerieModalComponent} from './components/invoice-serie-modal/invoice-serie-modal.component';


@NgModule({
  declarations: [
    CompanyComponent,
    WarehouseListComponent,
    WarehouseModalComponent,
    InvoiceSerieListComponent,
    InvoiceSerieModalComponent
  ],
  imports: [
    CommonModule,
    SystemRoutingModule,
    ReactiveFormsModule,
    GlobalModule,
    FontAwesomeModule
  ]
})
export class SystemModule {
}
