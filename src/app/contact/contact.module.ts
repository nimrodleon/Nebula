import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';

import {ContactRoutingModule} from './contact-routing.module';
import {GlobalModule} from '../global/global.module';
import {ContactListComponent} from './pages/contact-list/contact-list.component';


@NgModule({
  declarations: [
    ContactListComponent
  ],
  imports: [
    CommonModule,
    ContactRoutingModule,
    FontAwesomeModule,
    GlobalModule
  ]
})
export class ContactModule {
}
