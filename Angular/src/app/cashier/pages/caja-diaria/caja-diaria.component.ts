import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {faBars, faPlus, faSyncAlt, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import * as moment from 'moment';
import {CajaDiariaService} from '../../services';
import {CajaDiaria} from '../../interfaces';
import {InvoiceSerie} from '../../../system/interfaces';
import {InvoiceSerieService} from '../../../system/services';

declare var bootstrap: any;

@Component({
  selector: 'app-caja-diaria',
  templateUrl: './caja-diaria.component.html',
  styleUrls: ['./caja-diaria.component.scss']
})
export class CajaDiariaComponent implements OnInit {
  faSyncAlt = faSyncAlt;
  faPlus = faPlus;
  faBars = faBars;
  faTrashAlt = faTrashAlt;
  // ====================================================================================================
  aperturaCajaModal: any;
  invoiceSeries: Array<InvoiceSerie> = new Array<InvoiceSerie>();
  cajasDiarias: Array<CajaDiaria> = new Array<CajaDiaria>();
  queryForm: FormGroup = this.fb.group({
    year: [moment().format('YYYY')],
    month: [moment().format('MM')],
  });
  // ====================================================================================================
  aperturaForm: FormGroup = this.fb.group({
    serieId: [''], total: 0
  });

  constructor(
    private fb: FormBuilder,
    private invoiceSerieService: InvoiceSerieService,
    private cajaDiariaService: CajaDiariaService) {
  }

  ngOnInit(): void {
    this.cargarCajasDiarias();
    // seleccionar modal apertura de caja.
    this.aperturaCajaModal = new bootstrap.Modal(document.querySelector('#aperturaCaja'));
    // reset aperturaForm al iniciar el modal.
    if (document.querySelector('#aperturaCaja')) {
      const myModal: any = document.querySelector('#aperturaCaja');
      myModal.addEventListener('hide.bs.modal', () => {
        this.aperturaForm.reset({serieId: '', total: 0});
      });
    }
    // cargar series de facturación.
    this.invoiceSerieService.index().subscribe(result => this.invoiceSeries = result);
  }

  // cargar cajas diarias.
  public cargarCajasDiarias(): void {
    const year = this.queryForm.get('year')?.value;
    const month = this.queryForm.get('month')?.value;
    this.cajaDiariaService.index(year, month).subscribe(result => this.cajasDiarias = result);
  }

  // botón apertura caja.
  public aperturaCajaClick(): void {
    this.aperturaCajaModal.show();
  }

  // guardar apertura de caja.
  public guardarAperturaCaja(): void {
    this.cajaDiariaService.create(this.aperturaForm.value)
      .subscribe(result => {
        if (result.ok) {
          this.cargarCajasDiarias();
          this.aperturaCajaModal.hide();
        }
      });
  }

}
