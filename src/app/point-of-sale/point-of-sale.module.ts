import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PointOfSaleRoutingModule } from './point-of-sale-routing.module';
import { CajaDiariaComponent } from './pages/caja-diaria/caja-diaria.component';
import {GlobalModule} from '../global/global.module';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';


@NgModule({
  declarations: [
    CajaDiariaComponent
  ],
  imports: [
    CommonModule,
    PointOfSaleRoutingModule,
    GlobalModule,
    FontAwesomeModule
  ]
})
export class PointOfSaleModule { }
