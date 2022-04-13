import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import * as _ from 'lodash';
import {CashierDetailService} from '../../services';
import {CashierDetail} from '../../interfaces';

@Component({
  selector: 'app-forma-pago-report',
  templateUrl: './forma-pago-report.component.html'
})
export class FormaPagoReportComponent implements OnInit {
  cashierDetails: Array<CashierDetail> = new Array<CashierDetail>();

  constructor(
    private activatedRoute: ActivatedRoute,
    private cashierDetailService: CashierDetailService) {
  }

  ngOnInit(): void {
    const id: string = this.activatedRoute.snapshot.params['id'];
    this.cashierDetailService.index(id).subscribe(result => this.cashierDetails = result);
  }

  public calcularMonto(
    typeOperation: 'APERTURA_DE_CAJA' | 'ENTRADA_DE_DINERO' | 'SALIDA_DE_DINERO' | 'COMPROBANTE_DE_VENTA',
    formaPago: 'Contado:Yape' | 'Credito:Crédito' | 'Contado:Contado' | 'Contado:Depósito'): number {
    const objFilter = _.filter(this.cashierDetails, (item: CashierDetail) => item.typeOperation === typeOperation && item.formaPago === formaPago);
    return _.sumBy(objFilter, (item: CashierDetail) => item.amount);
  }

  public calcularTotales(formaPago: 'Contado:Yape' | 'Credito:Crédito' | 'Contado:Contado' | 'Contado:Depósito'): number {
    const objFilter = _.filter(this.cashierDetails, (item: CashierDetail) => item.typeOperation !== 'SALIDA_DE_DINERO' && item.formaPago === formaPago);
    return _.sumBy(objFilter, (item: CashierDetail) => item.amount);
  }
}
