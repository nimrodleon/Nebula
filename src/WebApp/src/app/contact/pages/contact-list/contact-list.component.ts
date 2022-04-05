import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl} from '@angular/forms';
import {faEdit, faFilter, faPlus, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {AuthService} from 'src/app/user/services';
import {accessDenied, deleteConfirm, ResponseData} from 'src/app/global/interfaces';
import {ContactService} from '../../services';
import {Contact} from '../../interfaces';

declare var bootstrap: any;

@Component({
  selector: 'app-contact-list',
  templateUrl: './contact-list.component.html'
})
export class ContactListComponent implements OnInit {
  faSearch = faSearch;
  faPlus = faPlus;
  faTrashAlt = faTrashAlt;
  faEdit = faEdit;
  faFilter = faFilter;
  // ====================================================================================================
  currentContact: Contact = new Contact();
  contacts: Array<Contact> = new Array<Contact>();
  query: FormControl = this.fb.control('');
  contactModal: any;
  title: string = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private contactService: ContactService) {
  }

  ngOnInit(): void {
    // modal formulario de contactos.
    this.contactModal = new bootstrap.Modal(document.querySelector('#contact-modal'));
    // cargar lista de contactos.
    this.cargarListaDeContactos();
  }

  // botón buscar contactos.
  public submitSearch(e: any): void {
    e.preventDefault();
    this.cargarListaDeContactos();
  }

  // cargar lista de contactos.
  private cargarListaDeContactos(): void {
    this.contactService.index(this.query.value)
      .subscribe(result => this.contacts = result);
  }

  // botón agregar contacto.
  public addContactModal(): void {
    this.title = 'Agregar Contacto';
    this.currentContact = new Contact();
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
      this.cargarListaDeContactos();
    }
  }

  // borrar contacto.
  public deleteContact(id: string): void {
    this.authService.getMe().subscribe(async (result) => {
      if (result.role !== 'Admin') {
        await accessDenied();
      } else {
        deleteConfirm().then(result => {
          if (result.isConfirmed) {
            this.contactService.delete(id).subscribe(result => {
              if (result.ok) this.cargarListaDeContactos();
            });
          }
        });
      }
    });
  }

}
