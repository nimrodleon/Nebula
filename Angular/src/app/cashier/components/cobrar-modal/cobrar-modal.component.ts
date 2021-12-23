import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {faBars, faEnvelope, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {faCheckSquare} from '@fortawesome/free-regular-svg-icons';
import {FormBuilder, FormGroup} from '@angular/forms';
import {FacturadorService} from 'src/app/invoice/services';
import {TerminalService} from '../../services';

@Component({
  selector: 'app-cobrar-modal',
  templateUrl: './cobrar-modal.component.html',
  styleUrls: ['./cobrar-modal.component.scss']
})
export class CobrarModalComponent implements OnInit {
  faBars = faBars;
  faCheckSquare = faCheckSquare;
  faEnvelope = faEnvelope;
  faTrashAlt = faTrashAlt;
  // ====================================================================================================
  @Input()
  cajaDiariaId: number = 0;
  @Output()
  responseData = new EventEmitter<boolean>();
  cobrarForm: FormGroup = this.fb.group({
    paymentMethod: ['0'],
    docType: ['BL'],
    montoTotal: [0],
    remark: ['']
  });
  formReg: boolean = true;

  constructor(
    private fb: FormBuilder,
    private terminalService: TerminalService,
    private facturadorService: FacturadorService) {
  }

  ngOnInit(): void {
    const myModal: any = document.querySelector('#cobrar-modal');
    myModal.addEventListener('shown.bs.modal', () => {
      this.formReg = true;
    });
    myModal.addEventListener('hide.bs.modal', () => {
      if (!this.formReg) {
        this.terminalService.deleteSale();
        this.responseData.emit(this.formReg);
      }
    });
  }

  public get sale() {
    return this.terminalService.sale;
  }

  public cobrarVenta(): void {
    this.terminalService.addInfo(this.cobrarForm.value);
    this.terminalService.saveChanges(this.cajaDiariaId).subscribe(result => {
      if (result.ok) {
        if (result.data) {
          const venta = result.data;
          this.terminalService.sale = result.data;
          this.facturadorService.GenerarComprobante(venta.invoiceId)
            .subscribe(result => {
              if (result.validacion === 'EXITO') {
                this.facturadorService.GenerarPdf(venta.invoiceId).subscribe(_ => this.formReg = false);
              }
            });
          // log información.
          console.log(result.data);
        }
      }
    }, ({error}) => {
      console.error(error);
    });
  }

}
