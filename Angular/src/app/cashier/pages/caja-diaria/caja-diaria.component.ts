import {Component, OnInit} from '@angular/core';
import {faBars, faPlus, faSignOutAlt, faSyncAlt, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {CajaService} from '../../services';
import {Caja} from '../../interfaces';

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

  constructor(private cajaService: CajaService) {
  }

  ngOnInit(): void {
    // seleccionar modal apertura de caja.
    this.aperturaCajaModal = new bootstrap.Modal(document.querySelector('#aperturaCaja'));
    // cargar lista de cajas.
    this.cajaService.index().subscribe(result => this.listaDeCajas = result);
  }

  // botón apertura caja.
  public aperturaCajaClick(): void {
    this.aperturaCajaModal.show();
  }

}
