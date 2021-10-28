import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {faBars} from '@fortawesome/free-solid-svg-icons';
import {Contact, PeopleDocType} from '../../interfaces';
import {ContactService, PeopleDocTypeService} from '../../services';
import {ResponseData} from 'src/app/global/interfaces';

@Component({
  selector: 'app-contact-modal',
  templateUrl: './contact-modal.component.html',
  styleUrls: ['./contact-modal.component.scss']
})
export class ContactModalComponent implements OnInit {
  faBars = faBars;
  // ====================================================================================================
  @Input()
  title: string = '';
  @Input()
  contact: Contact = new Contact();
  @Output()
  responseData = new EventEmitter<ResponseData<Contact>>();
  // ====================================================================================================
  docTypes: Array<PeopleDocType> = new Array<PeopleDocType>();
  contactForm: FormGroup = this.fb.group({
    id: [null],
    document: [''],
    typeDoc: [''],
    name: [''],
    address: [''],
    phoneNumber1: [''],
    phoneNumber2: [''],
    email: [''],
  });

  constructor(
    private fb: FormBuilder,
    private contactService: ContactService,
    private peopleDocTypeService: PeopleDocTypeService) {
  }

  ngOnInit(): void {
    // suscribir formGroup.
    this.contactForm.valueChanges.subscribe(value => this.contact = value);
    // cargar lista de tipos de documento.
    this.peopleDocTypeService.index().subscribe(result => this.docTypes = result);
    if (document.querySelector('#contact-modal')) {
      const myModal: any = document.querySelector('#contact-modal');
      myModal.addEventListener('shown.bs.modal', () => {
        this.contactForm.reset({...this.contact});
      });
    }
  }

  // guardar cambios del formulario.
  public saveChanges(): void {
    if (this.contactForm.get('id')?.value == null) {
      this.contact.id = undefined;
      this.contactService.store(this.contact)
        .subscribe(result => this.responseData.emit(result));
    } else {
      const id = this.contact.id;
      this.contactService.update(<any>id, this.contact)
        .subscribe(result => this.responseData.emit(result));
    }
  }

}
