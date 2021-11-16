import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ReactiveFormsModule} from '@angular/forms';

import {ShoppingRoutingModule} from './shopping-routing.module';
import {ShoppingListComponent} from './pages/shopping-list/shopping-list.component';
import {GlobalModule} from '../global/global.module';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {InvoiceNoteListComponent} from './pages/invoice-note-list/invoice-note-list.component';

@NgModule({
  declarations: [
    ShoppingListComponent,
    InvoiceNoteListComponent
  ],
  imports: [
    CommonModule,
    ShoppingRoutingModule,
    ReactiveFormsModule,
    GlobalModule,
    FontAwesomeModule
  ]
})
export class ShoppingModule {
}
