import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ReactiveFormsModule} from '@angular/forms';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';

import {ShoppingRoutingModule} from './shopping-routing.module';
import {ShoppingListComponent} from './pages/shopping-list/shopping-list.component';
import {GlobalModule} from '../global/global.module';
import {InvoiceNoteListComponent} from './pages/invoice-note-list/invoice-note-list.component';
import {AccountPayableListComponent} from './pages/account-payable-list/account-payable-list.component';

@NgModule({
  declarations: [
    ShoppingListComponent,
    InvoiceNoteListComponent,
    AccountPayableListComponent
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
