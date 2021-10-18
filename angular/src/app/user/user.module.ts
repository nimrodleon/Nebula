import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {UserRoutingModule} from './user-routing.module';
import {AuthService} from './services';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    UserRoutingModule
  ],
  providers: [
    AuthService
  ]
})
export class UserModule {
}
