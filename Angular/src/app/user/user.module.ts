import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';

import {UserRoutingModule} from './user-routing.module';
import {GlobalModule} from '../global/global.module';
import {AuthService} from './services';
import {UserListComponent} from './pages/user-list/user-list.component';
import {UserModalComponent} from './components/user-modal/user-modal.component';
import {ChangePasswordComponent} from './components/change-password/change-password.component';


@NgModule({
  declarations: [
    UserListComponent,
    UserModalComponent,
    ChangePasswordComponent
  ],
  imports: [
    CommonModule,
    UserRoutingModule,
    GlobalModule,
    FontAwesomeModule
  ],
  providers: [
    AuthService
  ]
})
export class UserModule {
}
