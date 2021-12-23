import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {faBars, faEnvelope, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {faCheckSquare} from '@fortawesome/free-regular-svg-icons';
import {FormBuilder, FormGroup} from '@angular/forms';
import {environment} from 'src/environments/environment';
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
  private appURL: string = environment.applicationUrl;
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
  urlPdf: string = '';

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
    // guardar comprobante de pago.
    this.terminalService.saveChanges(this.cajaDiariaId)
      .subscribe(result => {
        if (result.ok) {
          if (result.data) {
            const {data} = result;
            this.terminalService.sale = data;
            this.urlPdf = `${this.appURL}Facturador/GetPdf?invoice=${data?.invoiceId}`;
            console.info(result.msg);
            // cargar la Lista de archivos JSON.
            this.facturadorService.ActualizarPantalla()
              .subscribe(result => {
                if (result.listaBandejaFacturador.length > 0) {
                  // generar fichero XML del comprobante.
                  this.facturadorService.GenerarComprobante(data.invoiceId)
                    .subscribe(result => {
                      if (result.listaBandejaFacturador.length > 0) {
                        this.formReg = false;
                        // generar fichero PDF del comprobante.
                        this.facturadorService.GenerarPdf(data.invoiceId).subscribe(console.info);
                      }
                    });
                }
              });
          }
        }
      });
  }

}
