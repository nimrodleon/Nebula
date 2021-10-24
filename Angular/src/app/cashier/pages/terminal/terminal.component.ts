import {Component, OnInit} from '@angular/core';
import {
  faBarcode, faBars,
  faCashRegister,
  faCoins, faLock, faMinus, faPlus, faPuzzlePiece,
  faSearch, faSignOutAlt, faTrashAlt, faUserCircle
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
  faPuzzlePiece = faPuzzlePiece;
  faSignOutAlt = faSignOutAlt;
  faCashRegister = faCashRegister;
  faBarcode = faBarcode;
  faBars = faBars;
  // ====================================================================================================
  bsOffcanvas: any;
  cobrarModal: any;
  cashInOutModal: any;

  constructor() {
  }

  ngOnInit(): void {
    // formulario modal cobrar.
    this.cobrarModal = new bootstrap.Modal(document.querySelector('#cobrar-modal'));
    // formulario entrada/salida de efectivo.
    this.cashInOutModal = new bootstrap.Modal(document.querySelector('#cash-in-out-modal'));
    // menu principal del punto de venta.
    if (document.getElementById('offcanvas')) {
      this.bsOffcanvas = new bootstrap.Offcanvas(document.getElementById('offcanvas'));
    }
  }

  // mostrar menu principal.
  public showMenuCanvas(e: any) {
    e.preventDefault();
    // mostrar menu principal del terminal.
    if (document.getElementById('offcanvas')) {
      this.bsOffcanvas.show();
    }
  }

  // btnCashInOutClick(): void {
  //   this.cashInOutModal.show();
  // }
  //
  // // botón vender.
  // btnVenderClick(): void {
  //   this.cobrarModal.show();
  // }

}
