import {Component, OnInit} from '@angular/core';
import {
  faCashRegister,
  faCog, faFilter, faLock, faMinusCircle,
  faPlus, faSearch, faTrashAlt
} from '@fortawesome/free-solid-svg-icons';
import {ActivatedRoute} from '@angular/router';
import {CashierDetail} from '../../interfaces';
import {CashierDetailService} from '../../services';
import {FormBuilder, FormControl} from '@angular/forms';

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
  cajaDiariaId: string = '';
  query: FormControl = this.fb.control('');
  cashierDetails: Array<CashierDetail> = new Array<CashierDetail>();

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private cashierDetailService: CashierDetailService) {
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.cajaDiariaId = params.get('id') || '';
      // cargar detalle de caja.
      this.loadCashierDetails();
    });
  }

  // cargar detalle de caja.
  private loadCashierDetails(): void {
    this.cashierDetailService.index(<any>this.cajaDiariaId, this.query.value)
      .subscribe(result => this.cashierDetails = result);
  }

  // buscar documentos.
  public submitSearch(e: any): void {
    e.preventDefault();
    this.loadCashierDetails();
  }


}
