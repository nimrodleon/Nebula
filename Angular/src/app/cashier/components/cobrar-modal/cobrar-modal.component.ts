import {Component, OnInit} from '@angular/core';
import {faBars, faEnvelope, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {faCheckSquare} from '@fortawesome/free-regular-svg-icons';
import {FormBuilder, FormGroup} from '@angular/forms';
import {Cuota} from '../../interfaces';
import {deleteConfirm} from '../../../global/interfaces';
import {TerminalService} from '../../services';

declare var bootstrap: any;

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
  cuotaModal: any;
  cobrarForm: FormGroup = this.fb.group({
    paymentType: ['Contado'],
    docType: ['NV'],
    endDate: [null],
    montoCaja: [true],
    montoTotal: [0],
    remark: ['']
  });
  listaDeCuotas: Array<Cuota> = new Array<Cuota>();

  constructor(
    private fb: FormBuilder,
    private terminalService: TerminalService) {
  }

  ngOnInit(): void {
    this.cuotaModal = new bootstrap.Modal(document.querySelector('#cuota-modal'));
  }

  // agregar cuota.
  public addCuotaModal(e: any): void {
    e.preventDefault();
    this.cuotaModal.show();
  }

  // ocultar cuota modal.
  public hideCuotaModal(data: Cuota): void {
    if (data) {
      data.numCuota = this.listaDeCuotas.length + 1;
      this.listaDeCuotas.push(data);
      this.cuotaModal.hide();
    }
  }

  // borrar item lista de cuotas.
  public deleteItemCuota(numCuota: number): void {
    deleteConfirm().then(result => {
      if (result.isConfirmed) {
        let deleted: Boolean = false;
        this.listaDeCuotas.forEach((value, index, array) => {
          if (value.numCuota === numCuota) {
            array.splice(index, 1);
            deleted = true;
          }
        });
        if (deleted) {
          for (let i = 0; i < this.listaDeCuotas.length; i++) {
            this.listaDeCuotas[i].numCuota = i + 1;
          }
        }
      }
    });
  }

  // cobrar venta.
  public cobrarVenta(): void {
    this.terminalService.addInfo(this.cobrarForm.value);
    this.terminalService.addCuotas(this.listaDeCuotas);
    console.log(JSON.stringify(this.cobrarForm.value));
  }

}
