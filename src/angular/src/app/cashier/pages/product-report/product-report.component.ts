import {Component, OnInit} from "@angular/core";
import {ActivatedRoute} from "@angular/router";
import {InvoiceSaleDetail} from "app/sales/interfaces";
import {InvoiceSaleCashierService} from "../../services";
import _ from "lodash";
import {CashierContainerComponent} from "app/common/containers/cashier-container/cashier-container.component";
import {CurrencyPipe, NgForOf} from "@angular/common";

@Component({
  selector: "app-product-report",
  standalone: true,
  imports: [
    CashierContainerComponent,
    NgForOf,
    CurrencyPipe
  ],
  templateUrl: "./product-report.component.html"
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
    const id: string = this.activatedRoute.snapshot.params["id"];
    this.invoiceSaleCashierService.productReport(id).subscribe(result => {
      let objArr = new Array<InvoiceSaleDetail>();
      const objFilter = _.groupBy(result, "codProducto");
      _.forEach(objFilter, (item: Array<InvoiceSaleDetail>) => {
        if (item.length === 1) {
          objArr = objArr.concat(item);
        }
        if (item.length > 1) {
          let objTmp = {...item[0]};
          objTmp.cantidad = _.sumBy(item, "cantidad");
          objTmp.mtoBaseIgv = _.sumBy(item, "mtoBaseIgv");
          objTmp.igv = _.sumBy(item, "igv");
          objArr = objArr.concat(objTmp);
        }
      });
      this.invoiceSaleDetails = objArr;
      this.totalBienes = this.calcularTotales(result, "BIEN");
      this.totalServicios = this.calcularTotales(result, "SERVICIO");
    });
  }

  private calcularTotales(data: Array<InvoiceSaleDetail>, type: "BIEN" | "SERVICIO"): number {
    const objFilter = _.filter(data, (item: InvoiceSaleDetail) => item.tipoItem === type);
    const mtoBaseIgv = _.sumBy(objFilter, "mtoBaseIgv");
    const igv = _.sumBy(objFilter, "igv");
    return mtoBaseIgv + igv;
  }

}
