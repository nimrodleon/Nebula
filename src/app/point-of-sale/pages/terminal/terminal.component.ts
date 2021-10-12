import {Component, OnInit} from '@angular/core';
import {
  faCoins, faLock, faMinus, faPlus,
  faSearch, faTrashAlt, faUserCircle
} from '@fortawesome/free-solid-svg-icons';

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
  faLock = faLock;
  faCoins = faCoins;
  faTrashAlt = faTrashAlt;
  faMinus = faMinus;
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

  btnCashInOutClick(): void {
    this.cashInOutModal.show();
  }

  // botón vender.
  btnVenderClick(): void {
    this.cobrarModal.show();
  }

}
