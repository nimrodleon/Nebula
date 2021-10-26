import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {faBars, faPlus, faSyncAlt, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import * as moment from 'moment';
import {CajaDiariaService, CajaService} from '../../services';
import {AperturaCaja, Caja, CajaDiaria} from '../../interfaces';

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
  listaDeCajas: Array<Caja> = new Array<Caja>();
  cajasDiarias: Array<CajaDiaria> = new Array<CajaDiaria>();
  queryForm: FormGroup = this.fb.group({
    year: [moment().format('YYYY')],
    month: [moment().format('MM')],
  });
  queryData: any = {
    year: moment().format('YYYY'),
    month: moment().format('MM'),
  };
  // ====================================================================================================
  aperturaForm: FormGroup = this.fb.group({
    cajaId: [''], total: 0
  });
  aperturaData: AperturaCaja = {
    cajaId: '', total: 0
  };

  constructor(
    private fb: FormBuilder,
    private cajaService: CajaService,
    private cajaDiariaService: CajaDiariaService) {
  }

  ngOnInit(): void {
    this.cargarCajasDiarias();
    // suscribir queryForm con queryData.
    this.queryForm.valueChanges.subscribe(value => this.queryData = value);
    // suscribir aperturaForm con aperturaData.
    this.aperturaForm.valueChanges.subscribe(value => this.aperturaData = value);
    // seleccionar modal apertura de caja.
    this.aperturaCajaModal = new bootstrap.Modal(document.querySelector('#aperturaCaja'));
    // reset aperturaForm al iniciar el modal.
    if (document.querySelector('#aperturaCaja')) {
      let myModalEl: any = document.querySelector('#aperturaCaja');
      myModalEl.addEventListener('shown.bs.modal', () => {
        this.aperturaForm.reset({cajaId: '', total: 0});
      });
    }
    // cargar lista de cajas.
    this.cajaService.index().subscribe(result => this.listaDeCajas = result);
  }

  // cargar cajas diarias.
  public cargarCajasDiarias(): void {
    this.cajaDiariaService.index(this.queryData.year, this.queryData.month)
      .subscribe(result => this.cajasDiarias = result);
  }

  // botón apertura caja.
  public aperturaCajaClick(): void {
    this.aperturaCajaModal.show();
  }

  // guardar apertura de caja.
  public guardarAperturaCaja(): void {
    this.cajaDiariaService.store(this.aperturaData)
      .subscribe(result => {
        if (result.ok) {
          this.aperturaCajaModal.hide();
          this.cargarCajasDiarias();
        }
      });
  }

}
