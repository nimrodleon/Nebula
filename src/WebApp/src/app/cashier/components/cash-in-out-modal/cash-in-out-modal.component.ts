import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {faBars} from '@fortawesome/free-solid-svg-icons';
import {environment} from 'src/environments/environment';
import {CashierDetail} from '../../interfaces';
import {CashierDetailService} from '../../services';
import {ResponseData} from 'src/app/global/interfaces';

declare var jQuery: any;

@Component({
  selector: 'app-cash-in-out-modal',
  templateUrl: './cash-in-out-modal.component.html'
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
    type: ['', [Validators.required]],
    contact: [null, [Validators.required]],
    amount: [null, [Validators.required, Validators.min(0.1)]],
    remark: ['', [Validators.required]],
  });

  constructor(
    private fb: FormBuilder,
    private cashierDetailService: CashierDetailService) {
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
      const text = data.text.split('-');
      this.cajaChicaForm.controls['contact'].setValue(`${data.id}:${text[text.length - 1].trim()}`);
    });
    // cargar valores por defecto.
    if (document.querySelector('#cash-in-out-modal')) {
      const myModal: any = document.querySelector('#cash-in-out-modal');
      myModal.addEventListener('hide.bs.modal', () => {
        jQuery('#contactId').val(null).trigger('change');
        this.cashierDetail = new CashierDetail();
        this.cashierDetail.cajaDiaria = this.cajaDiariaId;
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
    this.cashierDetail = {...this.cajaChicaForm.value};
    this.cashierDetail.cajaDiaria = this.cajaDiariaId;
    this.cashierDetailService.create(this.cashierDetail)
      .subscribe(result => this.responseData.emit(result));
  }

}
