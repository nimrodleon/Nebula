import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {faBars} from '@fortawesome/free-solid-svg-icons';
import {Contact} from '../../interfaces';
import {ContactService} from '../../services';
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
  contact: Contact | any;
  @Output()
  responseData = new EventEmitter<ResponseData<Contact>>();
  // ====================================================================================================
  contactForm: FormGroup = this.fb.group({
    id: [null],
    document: ['', [Validators.required]],
    docType: [0, [Validators.required]],
    name: ['', [Validators.required]],
    address: ['', [Validators.required]],
    phoneNumber: ['', [Validators.required]],
    email: ['', [Validators.required]],
  });

  constructor(
    private fb: FormBuilder,
    private contactService: ContactService) {
  }

  ngOnInit(): void {
    // suscribir formGroup.
    this.contactForm.valueChanges.subscribe(value => this.contact = value);
    if (document.querySelector('#contact-modal')) {
      const myModal: any = document.querySelector('#contact-modal');
      myModal.addEventListener('shown.bs.modal', () => {
        this.contactForm.reset({...this.contact});
      });
      myModal.addEventListener('hide.bs.modal', () => {
        this.contactForm.reset();
      });
    }
  }

  // Verificar campo invalido.
  public inputIsInvalid(field: string) {
    return this.contactForm.controls[field].errors && this.contactForm.controls[field].touched;
  }

  // guardar cambios del formulario.
  public saveChanges(): void {
    if (this.contactForm.invalid) {
      this.contactForm.markAllAsTouched();
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    if (this.contactForm.get('id')?.value == null) {
      this.contact.id = undefined;
      this.contactService.create(this.contact)
        .subscribe(result => this.responseData.emit(result));
    } else {
      const id = this.contact.id;
      this.contactService.update(<any>id, this.contact)
        .subscribe(result => this.responseData.emit(result));
    }
  }

}
