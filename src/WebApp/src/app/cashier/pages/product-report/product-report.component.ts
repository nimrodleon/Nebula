import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import * as _ from 'lodash';
import {InvoiceSaleDetail} from 'src/app/sales/interfaces';
import {InvoiceSaleCashierService} from '../../services';

@Component({
  selector: 'app-product-report',
  templateUrl: './product-report.component.html'
})
export class ProductReportComponent implements OnInit {
  totalBienes: number = 0;
  totalServicios: number = 0;
  invoiceSaleDetails: Array<InvoiceSaleDetail> = new Array<InvoiceSaleDetail>();

  constructor(
    private activatedRoute: ActivatedRoute,
    private invoiceSaleCashierService: InvoiceSaleCashierService) {
  }

  ngOnInit(): void {
    const id: string = this.activatedRoute.snapshot.params['id'];
    this.invoiceSaleCashierService.productReport(id).subscribe(result => {
      let objArr = new Array<InvoiceSaleDetail>();
      const objFilter = _.groupBy(result, 'codProducto');
      _.forEach(objFilter, (item: Array<InvoiceSaleDetail>) => {
        if (item.length === 1) {
          objArr = objArr.concat(item);
        }
        if (item.length > 1) {
          let objTmp = {...item[0]};
          objTmp.ctdUnidadItem = _.sumBy(item, 'ctdUnidadItem');
          objTmp.mtoBaseIgvItem = _.sumBy(item, 'mtoBaseIgvItem');
          objTmp.mtoIgvItem = _.sumBy(item, 'mtoIgvItem');
          objTmp.mtoTriIcbperItem = _.sumBy(item, 'mtoTriIcbperItem');
          objArr = objArr.concat(objTmp);
        }
      });
      this.invoiceSaleDetails = objArr;
      this.totalBienes = this.calcularTotales(result, 'BIEN');
      this.totalServicios = this.calcularTotales(result, 'SERVICIO');
    });
  }

  private calcularTotales(data: Array<InvoiceSaleDetail>, type: 'BIEN' | 'SERVICIO'): number {
    const objFilter = _.filter(data, (item: InvoiceSaleDetail) => item.tipoItem === type);
    const mtoBaseIgvItem = _.sumBy(objFilter, 'mtoBaseIgvItem');
    const mtoIgvItem = _.sumBy(objFilter, 'mtoIgvItem');
    const mtoTriIcbperItem = _.sumBy(objFilter, 'mtoTriIcbperItem');
    return mtoBaseIgvItem + mtoIgvItem + mtoTriIcbperItem;
  }

}
