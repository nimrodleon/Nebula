import {Component, OnInit} from "@angular/core";
import {FormBuilder, FormControl, ReactiveFormsModule} from "@angular/forms";
import {faEdit, faFilter, faPlus, faSearch, faTrashAlt} from "@fortawesome/free-solid-svg-icons";
import {PaginationResult, accessDenied, deleteConfirm, toastSuccess} from "app/common/interfaces";
import {UserDataService} from "app/common/user-data.service";
import {ContactService} from "../../services";
import {Contact, ContactDataModal} from "../../interfaces";
import _ from "lodash";
import {ContactContainerComponent} from "app/common/containers/contact-container/contact-container.component";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {SplitPipe} from "../../../common/pipes/split.pipe";
import {NgClass, NgForOf} from "@angular/common";
import {ActivatedRoute, RouterLink} from "@angular/router";
import {ContactModalComponent} from "app/common/contact/contact-modal/contact-modal.component";

declare const bootstrap: any;

@Component({
  selector: "app-contact-list",
  standalone: true,
  imports: [
    ContactContainerComponent,
    FaIconComponent,
    ReactiveFormsModule,
    SplitPipe,
    NgForOf,
    NgClass,
    RouterLink,
    ContactModalComponent
  ],
  templateUrl: "./contact-list.component.html"
})
export class ContactListComponent implements OnInit {
  faSearch = faSearch;
  faPlus = faPlus;
  faTrashAlt = faTrashAlt;
  faEdit = faEdit;
  faFilter = faFilter;
  // ====================================================================================================
  contactos: PaginationResult<Contact> = new PaginationResult<Contact>();
  query: FormControl = this.fb.control("");
  contactModal: any;
  contactDataModal = new ContactDataModal();

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private userDataService: UserDataService,
    private contactService: ContactService) {
  }

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      const page = params["page"];
      this.cargarListaDeContactos(page);
    });
    // modal formulario de contactos.
    this.contactModal = new bootstrap.Modal("#contact-modal");
  }

  public get companyName(): string {
    return this.userDataService.localName;
  }

  // botón buscar contactos.
  public submitSearch(e: any): void {
    e.preventDefault();
    this.cargarListaDeContactos();
  }

  // cargar lista de contactos.
  private cargarListaDeContactos(page: number = 1): void {
    this.contactService.index(this.query.value, page)
      .subscribe(result => this.contactos = result);
  }

  // botón agregar contacto.
  public addContactModal(): void {
    this.contactDataModal.type = "ADD";
    this.contactDataModal.title = "Agregar Contacto";
    this.contactDataModal.contact = new Contact();
    this.contactModal.show();
  }

  // botón editar contacto.
  public editContactModal(contact: Contact): void {
    this.contactDataModal.type = "EDIT";
    this.contactDataModal.title = "Editar Contacto";
    this.contactDataModal.contact = contact;
    this.contactModal.show();
  }

  // guardar datos de contacto.
  public saveChangesDetail(data: ContactDataModal): void {
    if (data.type === "ADD") {
      this.contactService.create(data.contact)
        .subscribe(result => {
          this.contactos.data = _.concat(result, this.contactos.data);
          this.contactModal.hide();
          toastSuccess("El contacto ha sido registrado!");
        });
    }
    if (data.type === "EDIT") {
      this.contactService.update(data.contact.id, data.contact)
        .subscribe(result => {
          this.contactos.data = _.map(this.contactos.data, item => {
            if (item.id === result.id) item = result;
            return item;
          });
          this.contactModal.hide();
          toastSuccess("El contacto ha sido actualizado!");
        });
    }
  }

  // borrar contacto.
  public deleteContact(id: string): void {
    if (!this.userDataService.canDelete()) {
      accessDenied().then(() => console.log("accessDenied"));
    } else {
      deleteConfirm().then(result => {
        if (result.isConfirmed) {
          this.contactService.delete(id).subscribe(result => {
            this.contactos.data = _.filter(this.contactos.data, item => item.id !== result.id);
          });
        }
      });
    }
  }

}
