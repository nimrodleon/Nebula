import {Component, OnInit} from '@angular/core';
import {faEdit, faFilter, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';

declare var bootstrap: any;

@Component({
  selector: 'app-contact-list',
  templateUrl: './contact-list.component.html',
  styleUrls: ['./contact-list.component.scss']
})
export class ContactListComponent implements OnInit {
  faSearch = faSearch;
  faPlus = faPlus;
  faTrashAlt = faTrashAlt;
  faEdit = faEdit;
  faFilter = faFilter;
  // ====================================================================================================
  contactModal: any;
  title: string = '';

  constructor() {
  }

  ngOnInit(): void {
    // modal formulario de contactos.
    this.contactModal = new bootstrap.Modal(document.querySelector('#contact-modal'));
  }

  // botón agregar contacto.
  addContact(): void {
    this.title = 'Agregar Contacto';
    this.contactModal.show();
  }

}
