import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Router} from '@angular/router';
import {faBars, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {faWhatsapp} from '@fortawesome/free-brands-svg-icons';
import {faCheckSquare} from '@fortawesome/free-regular-svg-icons';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
// import {environment} from 'src/environments/environment';
import {confirmTask} from 'src/app/global/interfaces';
import {InvoiceSaleCashierService} from '../../services';
import {GenerarVenta, ResponseCobrarModal} from '../../interfaces';

@Component({
  selector: 'app-cobrar-modal',
  templateUrl: './cobrar-modal.component.html',
  styleUrls: ['./cobrar-modal.component.scss']
})
export class CobrarModalComponent implements OnInit {
  faBars = faBars;
  faCheckSquare = faCheckSquare;
  faTrashAlt = faTrashAlt;
  faWhatsapp = faWhatsapp;
  // ====================================================================================================
  // private appURL: string = environment.applicationUrl;
  @Input() modal: any;
  @Input() cajaDiariaId: string = '';
  @Input() generarVenta: GenerarVenta = new GenerarVenta();
  @Output() responseData = new EventEmitter<ResponseCobrarModal>();
  cobrarForm: FormGroup = this.fb.group({
    formaPago: ['Contado:Contado', [Validators.required]],
    docType: ['NOTA', [Validators.required]],
    montoRecibido: [null, [Validators.required, Validators.min(0)]],
    remark: ['']
  });
  responseCobrarModal: ResponseCobrarModal = new ResponseCobrarModal();
  formReg: boolean = true;
  urlPdf: string = '';
  statusProgress: boolean = false;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private invoiceSaleCashierService: InvoiceSaleCashierService) {
  }

  ngOnInit(): void {
    const myModal: any = document.querySelector('#cobrar-modal');
    myModal.addEventListener('hide.bs.modal', () => {
      if (this.responseCobrarModal.status === 'COMPLETE'
        || this.responseCobrarModal.status === 'PRINT') {
        this.responseData.emit(this.responseCobrarModal);
        this.cobrarForm.reset({formaPago: 'Contado:Contado', docType: 'NOTA', montoRecibido: null, remark: ''});
      }
    });
    myModal.addEventListener('hidden.bs.modal', () => {
      if (this.responseCobrarModal.status === 'COMPLETE'
        || this.responseCobrarModal.status === 'PRINT') {
        this.responseCobrarModal = new ResponseCobrarModal();
        this.formReg = true;
      }
    });
  }

  // Verificar campo invalido.
  public inputIsInvalid(field: string) {
    return this.cobrarForm.controls[field].errors && this.cobrarForm.controls[field].touched;
  }

  public cobrarVenta(): void {
    if (this.cobrarForm.invalid) {
      this.cobrarForm.markAllAsTouched();
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    confirmTask().then(result => {
      if (result.isConfirmed) {
        this.statusProgress = true;
        this.generarVenta.comprobante = {...this.generarVenta.comprobante, ...this.cobrarForm.value};
        this.invoiceSaleCashierService.generarVenta(this.cajaDiariaId, this.generarVenta).subscribe(result => {
          if (result.ok) {
            this.generarVenta.comprobante = <any>result.data;
            this.responseCobrarModal.data = <any>result.data;
            this.responseCobrarModal.status = 'COMPLETE';
            this.statusProgress = false;
            this.formReg = false;
          }
        });
      }
    });
  }

  public print(): void {
    this.responseCobrarModal.status = 'PRINT';
    this.modal.hide();
  }

}
