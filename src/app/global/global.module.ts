import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {NavbarComponent} from './navbar/navbar.component';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {RouterModule} from '@angular/router';

@NgModule({
  declarations: [
    NavbarComponent
  ],
    imports: [
        CommonModule,
        FontAwesomeModule,
        RouterModule
    ],
  exports: [
    NavbarComponent
  ]
})
export class GlobalModule {
}
