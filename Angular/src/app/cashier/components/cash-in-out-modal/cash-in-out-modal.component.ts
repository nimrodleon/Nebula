import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {faBars} from '@fortawesome/free-solid-svg-icons';
import {environment} from 'src/environments/environment';
import {CashierDetail} from '../../interfaces';
import {ContactService} from 'src/app/contact/services';
import {CashierDetailService} from '../../services';
import {ResponseData} from 'src/app/global/interfaces';

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
  cajaDiariaId: number = 0;
  @Output()
  responseData = new EventEmitter<ResponseData<CashierDetail>>();
  cashierDetail: CashierDetail = new CashierDetail();
  cajaChicaForm: FormGroup = this.fb.group({
    id: [null],
    contactId: [null, [Validators.required]],
    glosa: ['', [Validators.required]],
    type: ['ENTRADA', [Validators.required]],
    total: [0, [Validators.required, Validators.min(0.1)]],
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
    }).on('select2:select', (e: any) => {
      const data = e.params.data;
      this.cajaChicaForm.controls['contactId'].setValue(data.id);
    });
    // cargar valores por defecto.
    if (document.querySelector('#cash-in-out-modal')) {
      const myModal: any = document.querySelector('#cash-in-out-modal');
      myModal.addEventListener('shown.bs.modal', () => {
        jQuery('#contactId').val(null).trigger('change');
        this.cashierDetail = new CashierDetail();
        this.cashierDetail.cajaDiariaId = <any>this.cajaDiariaId;
        this.cajaChicaForm.reset();
      });
    }
  }

  // Verificar campo invalido.
  public inputIsInvalid(field: string) {
    return this.cajaChicaForm.controls[field].errors && this.cajaChicaForm.controls[field].touched;
  }

  // guardar cambios.
  public saveChanges(): void {
    if (this.cajaChicaForm.invalid) {
      this.cajaChicaForm.markAllAsTouched();
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    const contactId = this.cajaChicaForm.get('contactId')?.value;
    this.cashierDetail = {...this.cajaChicaForm.value};
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
