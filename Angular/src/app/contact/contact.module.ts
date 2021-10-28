import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FontAwesomeModule} from '@fortawesome/angular-fontawesome';
import {ReactiveFormsModule} from '@angular/forms';

import {ContactRoutingModule} from './contact-routing.module';
import {GlobalModule} from '../global/global.module';
import {ContactListComponent} from './pages/contact-list/contact-list.component';
import {ContactModalComponent} from './components/contact-modal/contact-modal.component';

@NgModule({
    declarations: [
        ContactListComponent,
        ContactModalComponent
    ],
    exports: [
        ContactModalComponent
    ],
    imports: [
        CommonModule,
        ContactRoutingModule,
        ReactiveFormsModule,
        FontAwesomeModule,
        GlobalModule
    ]
})
export class ContactModule {
}
