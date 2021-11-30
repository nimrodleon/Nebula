import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl} from '@angular/forms';
import {faEdit, faFilter, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {ContactService} from '../../services';
import {PagedResponse, ResponseData} from '../../../global/interfaces';
import {Contact} from '../../interfaces';

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
  currentContact: Contact | any;
  contacts: PagedResponse<Contact> = new PagedResponse<Contact>();
  query: FormControl = this.fb.control('');
  pageNumber: number = 1;
  pageSize: number = 25;
  contactModal: any;
  title: string = '';

  constructor(
    private fb: FormBuilder,
    private contactService: ContactService) {
  }

  ngOnInit(): void {
    // modal formulario de contactos.
    this.contactModal = new bootstrap.Modal(document.querySelector('#contact-modal'));
    // cargar lista de contactos.
    this.loadContactList();
  }

  // botón buscar contactos.
  public submitSearch(e: any): void {
    e.preventDefault();
    this.loadContactList();
  }

  // cargar lista de contactos.
  private loadContactList(): void {
    this.contactService.index(this.pageNumber, this.pageSize, this.query.value)
      .subscribe(result => this.contacts = result);
  }

  // botón agregar contacto.
  public addContactModal(): void {
    this.title = 'Agregar Contacto';
    this.currentContact = null;
    this.contactModal.show();
  }

  // botón editar contacto.
  public editContactModal(id: any): void {
    this.title = 'Editar Contacto';
    this.contactService.show(id).subscribe(result => {
      this.currentContact = result;
      this.contactModal.show();
    });
  }

  // ocultar modal contacto.
  public hideContactModal(data: ResponseData<Contact>): void {
    if (data.ok) {
      this.contactModal.hide();
      this.loadContactList();
    }
  }

}
