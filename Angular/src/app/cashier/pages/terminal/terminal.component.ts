import {Component, OnInit} from '@angular/core';
import {
  faBarcode, faBars,
  faCashRegister, faCogs,
  faCoins, faIdCardAlt, faMinus, faPlus,
  faSearch, faSignOutAlt, faTags, faTimes, faTrashAlt, faUserCircle
} from '@fortawesome/free-solid-svg-icons';
import {faIntercom} from '@fortawesome/free-brands-svg-icons';

declare var bootstrap: any;

@Component({
  selector: 'app-terminal',
  templateUrl: './terminal.component.html',
  styleUrls: ['./terminal.component.scss']
})
export class TerminalComponent implements OnInit {
  faUserCircle = faUserCircle;
  faPlus = faPlus;
  faSearch = faSearch;
  faCoins = faCoins;
  faTrashAlt = faTrashAlt;
  faMinus = faMinus;
  faSignOutAlt = faSignOutAlt;
  faCashRegister = faCashRegister;
  faBarcode = faBarcode;
  faBars = faBars;
  faTags = faTags;
  faIntercom = faIntercom;
  faIdCardAlt = faIdCardAlt;
  faTimes = faTimes;
  faCogs = faCogs;
  // ====================================================================================================
  cobrarModal: any;
  cashInOutModal: any;

  constructor() {
  }

  ngOnInit(): void {
    // formulario modal cobrar.
    this.cobrarModal = new bootstrap.Modal(document.querySelector('#cobrar-modal'));
    // formulario entrada/salida de efectivo.
    this.cashInOutModal = new bootstrap.Modal(document.querySelector('#cash-in-out-modal'));
  }

  // movimientos de efectivo.
  btnCashInOutClick(): void {
    this.cashInOutModal.show();
  }

  // botón vender.
  btnVenderClick(): void {
    this.cobrarModal.show();
  }

}
