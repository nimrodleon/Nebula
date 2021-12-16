import {Component, Input, OnInit} from '@angular/core';
import {faBars, faEnvelope, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {faCheckSquare} from '@fortawesome/free-regular-svg-icons';
import {FormBuilder, FormGroup} from '@angular/forms';
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
  cobrarForm: FormGroup = this.fb.group({
    paymentMethod: ['0'],
    docType: ['BL'],
    montoTotal: [0],
    remark: ['']
  });
  formReg: boolean = true;

  constructor(
    private fb: FormBuilder,
    private terminalService: TerminalService) {
  }

  ngOnInit(): void {
    const myModal: any = document.querySelector('#cobrar-modal');
    myModal.addEventListener('hide.bs.modal', () => {
      if (!this.formReg) {
        this.terminalService.deleteSale();
        this.formReg = true;
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
          this.terminalService.sale = result.data;
        }
        this.formReg = false;
      }
    }, ({error}) => {
      console.error(error);
    });
  }

}
