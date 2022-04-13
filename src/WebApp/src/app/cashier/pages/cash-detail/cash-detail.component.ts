import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {FormBuilder, FormControl} from '@angular/forms';
import {
  faCashRegister,
  faChartBar, faCog, faFilter,
  faLock, faPlus, faSearch,
  faTrashAlt
} from '@fortawesome/free-solid-svg-icons';
import Swal from 'sweetalert2';
import * as _ from 'lodash';
import {CajaDiaria, CashierDetail} from '../../interfaces';
import {CajaDiariaService, CashierDetailService} from '../../services';
import {ResponseData} from 'src/app/global/interfaces';

declare var bootstrap: any;

@Component({
  selector: 'app-cash-detail',
  templateUrl: './cash-detail.component.html'
})
export class CashDetailComponent implements OnInit {
  faPlus = faPlus;
  faTrashAlt = faTrashAlt;
  faLock = faLock;
  faSearch = faSearch;
  faCashRegister = faCashRegister;
  faCog = faCog;
  faFilter = faFilter;
  faChartBar = faChartBar;
  // ====================================================================================================
  cajaDiaria: CajaDiaria = new CajaDiaria();
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
    const id: string = this.activatedRoute.snapshot.params['id'];
    this.cajaDiariaService.show(id).subscribe(result => {
      this.cajaDiaria = result;
      this.loadCashierDetails();
    });
    this.cerrarCajaModal = new bootstrap.Modal(document.querySelector('#cerrar-caja'));
  }

  // cargar detalle de caja.
  private loadCashierDetails(): void {
    this.cashierDetailService.index(this.cajaDiaria.id, this.query.value)
      .subscribe(result => this.cashierDetails = result);
  }

  // abrir terminal de venta.
  public async openTerminal() {
    if (this.cajaDiaria.status == 'ABIERTO') {
      await this.router.navigate(['/cashier/terminal', this.cajaDiaria.id]);
    } else {
      await Swal.fire(
        'Info?',
        'La caja esta cerrada!',
        'info'
      );
    }
  }

  // buscar documentos.
  public submitSearch(e: Event): void {
    e.preventDefault();
    this.loadCashierDetails();
  }

  // botón cerrar caja.
  public async showCerrarCajaModal() {
    if (this.cajaDiaria.status === 'ABIERTO') {
      this.cerrarCajaModal.show();
    } else {
      await Swal.fire(
        'Info?',
        'La caja esta cerrada!',
        'info'
      );
    }
  }

  // cerrar modal cierre de caja.
  public async hideCerrarCajaModal(result: ResponseData<CajaDiaria>) {
    if (result.ok) {
      this.cerrarCajaModal.hide();
      await this.router.navigate(['/cashier']);
    }
  }

  public async reporteCaja() {
    const calcularTotales = (formaPago: 'Contado:Yape' | 'Credito:Crédito' | 'Contado:Contado' | 'Contado:Depósito') => {
      const objFilter = _.filter(this.cashierDetails, (item: CashierDetail) => item.typeOperation !== 'SALIDA_DE_DINERO' && item.formaPago === formaPago);
      return _.sumBy(objFilter, (item: CashierDetail) => item.amount);
    };
    const totalSalidas = _.sumBy(_.filter(this.cashierDetails, (item: CashierDetail) =>
      item.typeOperation === 'SALIDA_DE_DINERO' && item.formaPago === 'Contado:Contado'), (item: CashierDetail) => item.amount);
    const totalContado = calcularTotales('Contado:Contado');
    await Swal.fire({
      title: '<strong class="text-primary text-uppercase"><u>Reporte</u></strong>',
      html: `
      <table class="table table-striped mb-0">
      <thead>
      <tr class="text-uppercase">
        <th>Concepto</th>
        <th>Total</th>
      </tr>
      </thead>
      <tbody class="text-uppercase">
      <tr>
        <td>Yape</td>
        <td>${calcularTotales('Contado:Yape').toFixed(2)}</td>
      </tr>
      <tr>
        <td>Crédito</td>
        <td>${calcularTotales('Credito:Crédito').toFixed(2)}</td>
      </tr>
      <tr>
        <td>Contado</td>
        <td>${totalContado.toFixed(2)}</td>
      </tr>
      <tr>
        <td>Depósito</td>
        <td>${calcularTotales('Contado:Depósito').toFixed(2)}</td>
      </tr>
      <tr class="bg-danger">
        <td class="text-white">Salida</td>
        <td class="text-white">${totalSalidas.toFixed(2)}</td>
      </tr>
      <tr class="bg-dark">
        <td class="text-white">Monto Total</td>
        <td class="text-white">${(totalContado - totalSalidas).toFixed(2)}</td>
      </tr>
      </tbody>
      </table>
      `,
      showCloseButton: true,
      showCancelButton: false,
      showConfirmButton: false
    });
  }

}
