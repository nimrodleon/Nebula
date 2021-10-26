import {Component, OnInit} from '@angular/core';
import {
  faCashRegister,
  faCog, faFilter, faLock, faMinusCircle,
  faPlus, faSearch, faTrashAlt
} from '@fortawesome/free-solid-svg-icons';
import {ActivatedRoute} from '@angular/router';

// declare var bootstrap: any;

@Component({
  selector: 'app-cash-detail',
  templateUrl: './cash-detail.component.html',
  styleUrls: ['./cash-detail.component.scss']
})
export class CashDetailComponent implements OnInit {
  faPlus = faPlus;
  faTrashAlt = faTrashAlt;
  faLock = faLock;
  faMinusCircle = faMinusCircle;
  faSearch = faSearch;
  faCashRegister = faCashRegister;
  faCog = faCog;
  faFilter = faFilter;
  // ====================================================================================================
  // cashInOutModal: any;
  cajaDiariaId: string = '';

  constructor(
    private activatedRoute: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.cajaDiariaId = params.get('id') || '';
    });
    // // modal entrada/salida de efectivo.
    // this.cashInOutModal = new bootstrap.Modal(
    //   document.querySelector('#cash-in-out-modal'));
  }

  // // Abrir modal entrada/salida de efectivo.
  // addCashInOut(e: any): void {
  //   e.preventDefault();
  //   this.cashInOutModal.show();
  // }

}
