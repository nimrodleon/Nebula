import {Component, OnInit} from '@angular/core';
import {faCoins, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';

declare var bootstrap: any;

@Component({
  selector: 'app-account-receivable-list',
  templateUrl: './account-receivable-list.component.html',
  styleUrls: ['./account-receivable-list.component.scss']
})
export class AccountReceivableListComponent implements OnInit {
  faSearch = faSearch;
  faTrashAlt = faTrashAlt;
  faCoins = faCoins;
  cobranzaModal: any;

  constructor() {
  }

  ngOnInit(): void {
    // formulario de cobranza.
    this.cobranzaModal = new bootstrap.Modal(document.querySelector('#cobranza-modal'));
  }

  // abrir modal cobranza.
  public showCobranzaModal(): void {
    this.cobranzaModal.show();
  }

}
