import {Component, OnInit} from '@angular/core';
import {
  faCashRegister,
  faCog, faFilter, faLock,
  faPlus, faSearch, faTrashAlt
} from '@fortawesome/free-solid-svg-icons';
import {ActivatedRoute, Router} from '@angular/router';
import {FormBuilder, FormControl} from '@angular/forms';
import Swal from 'sweetalert2';
import {CajaDiaria, CashierDetail} from '../../interfaces';
import {CajaDiariaService, CashierDetailService} from '../../services';
import {ResponseData} from '../../../global/interfaces';

declare var bootstrap: any;

@Component({
  selector: 'app-cash-detail',
  templateUrl: './cash-detail.component.html',
  styleUrls: ['./cash-detail.component.scss']
})
export class CashDetailComponent implements OnInit {
  faPlus = faPlus;
  faTrashAlt = faTrashAlt;
  faLock = faLock;
  faSearch = faSearch;
  faCashRegister = faCashRegister;
  faCog = faCog;
  faFilter = faFilter;
  // ====================================================================================================
  cajaDiariaId: string = '';
  // currentCajaDiaria: CajaDiaria = new CajaDiaria();
  query: FormControl = this.fb.control('');
  cashierDetails: Array<CashierDetail> = new Array<CashierDetail>();
  cerrarCajaModal: any;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private cajaDiariaService: CajaDiariaService,
    private cashierDetailService: CashierDetailService) {
  }

  ngOnInit(): void {
    // this.activatedRoute.paramMap.subscribe(params => {
    //   this.cajaDiariaId = params.get('id') || '';
    //   this.cajaDiariaService.show(<any>params.get('id'))
    //     .subscribe(result => this.currentCajaDiaria = result);
    //   // cargar detalle de caja.
    //   this.loadCashierDetails();
    // });
    // modal cerrar caja.
    this.cerrarCajaModal = new bootstrap.Modal(document.querySelector('#cerrar-caja'));
  }

  // cargar detalle de caja.
  private loadCashierDetails(): void {
    this.cashierDetailService.index(<any>this.cajaDiariaId, this.query.value)
      .subscribe(result => this.cashierDetails = result);
  }

  // abrir terminal de venta.
  public async openTerminal() {
    // if (this.currentCajaDiaria.state == 'ABIERTO') {
    //   await this.router.navigate(['/cashier/terminal', this.cajaDiariaId]);
    // } else {
    //   await Swal.fire(
    //     'Info?',
    //     'La caja esta cerrada!',
    //     'info'
    //   );
    // }
  }

  // buscar documentos.
  public submitSearch(e: any): void {
    e.preventDefault();
    this.loadCashierDetails();
  }

  // botón cerrar caja.
  public async showCerrarCajaModal() {
    // if (this.currentCajaDiaria.state === 'ABIERTO') {
    //   this.cerrarCajaModal.show();
    // } else {
    //   await Swal.fire(
    //     'Info?',
    //     'La caja esta cerrada!',
    //     'info'
    //   );
    // }
  }

  // cerrar modal cierre de caja.
  public async hideCerrarCajaModal(data: ResponseData<CajaDiaria>) {
    if (data.ok) {
      this.cerrarCajaModal.hide();
      await this.router.navigate(['/cashier']);
    }
  }

}
