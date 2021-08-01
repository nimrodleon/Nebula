import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {RouterModule} from '@angular/router';
import {NavbarComponent} from './navbar/navbar.component';
import {SystemOptionComponent} from './system-option/system-option.component';

@NgModule({
  declarations: [
    NavbarComponent,
    SystemOptionComponent
  ],
  imports: [
    CommonModule,
    FontAwesomeModule,
    RouterModule
  ],
  exports: [
    NavbarComponent,
    SystemOptionComponent
  ]
})
export class GlobalModule {
}
