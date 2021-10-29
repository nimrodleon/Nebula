import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {environment} from 'src/environments/environment';
import {faBars} from '@fortawesome/free-solid-svg-icons';
import {CashierDetail} from '../../interfaces';
import {FormBuilder, FormGroup} from '@angular/forms';
import {ContactService} from '../../../contact/services';
import {CashierDetailService} from '../../services';
import {ResponseData} from '../../../global/interfaces';

declare var jQuery: any;

@Component({
  selector: 'app-cash-in-out-modal',
  templateUrl: './cash-in-out-modal.component.html',
  styleUrls: ['./cash-in-out-modal.component.scss']
})
export class CashInOutModalComponent implements OnInit {
  private appURL: string = environment.applicationUrl;
  faBars = faBars;
  @Input()
  cajaDiariaId: string = '';
  @Output()
  responseData = new EventEmitter<ResponseData<CashierDetail>>();
  cashierDetail: CashierDetail = new CashierDetail();
  cajaChicaForm: FormGroup = this.fb.group({
    id: [null],
    cajaDiariaId: [''],
    contact: [''],
    glosa: [''],
    type: [''],
    total: 0,
  });

  constructor(
    private fb: FormBuilder,
    private cashierDetailService: CashierDetailService,
    private contactService: ContactService) {
  }

  ngOnInit(): void {
    jQuery('#contactId').select2({
      theme: 'bootstrap-5',
      placeholder: 'BUSCAR CONTACTO',
      dropdownParent: jQuery('#cash-in-out-modal'),
      ajax: {
        url: this.appURL + 'Contact/Select2',
        headers: {
          Authorization: 'Bearer ' + localStorage.getItem('token')
        }
      }
    });
    // suscribir cambios del formulario.
    this.cajaChicaForm.valueChanges
      .subscribe(value => this.cashierDetail = value);
    // cargar valores por defecto.
    if (document.querySelector('#cash-in-out-modal')) {
      const myModal: any = document.querySelector('#cash-in-out-modal');
      myModal.addEventListener('shown.bs.modal', () => {
        jQuery('#contactId').val(null).trigger('change');
        this.cashierDetail = new CashierDetail();
        this.cashierDetail.cajaDiariaId = <any>this.cajaDiariaId;
        this.cajaChicaForm.reset({...this.cashierDetail});
      });
    }
  }

  // guardar cambios.
  public saveChanges(): void {
    const contactId = jQuery('#contactId').val();
    this.contactService.show(contactId).subscribe(result => {
      this.cashierDetail.id = undefined;
      this.cashierDetail.cajaDiariaId = <any>this.cajaDiariaId;
      this.cashierDetail.contact = result.name;
      this.cashierDetail.document = '-';
      this.cashierDetailService.store(this.cashierDetail)
        .subscribe(result => this.responseData.emit(result));
    });
  }

}
