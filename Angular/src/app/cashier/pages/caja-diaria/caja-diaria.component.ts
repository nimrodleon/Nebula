import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {faBars, faPlus, faSignOutAlt, faSyncAlt, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import * as moment from 'moment';
import {CajaDiariaService, CajaService} from '../../services';
import {Caja, CajaDiaria} from '../../interfaces';

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
  faSignOutAlt = faSignOutAlt;
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

  constructor(
    private fb: FormBuilder,
    private cajaService: CajaService,
    private cajaDiariaService: CajaDiariaService) {
  }

  ngOnInit(): void {
    this.cargarCajasDiarias();
    // suscribir formGroup con queryData.
    this.queryForm.valueChanges.subscribe(value => this.queryData = value);
    // seleccionar modal apertura de caja.
    this.aperturaCajaModal = new bootstrap.Modal(document.querySelector('#aperturaCaja'));
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

}
