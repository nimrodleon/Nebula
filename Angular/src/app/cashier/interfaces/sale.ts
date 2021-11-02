import {SaleDetail} from './sale-detail';

export class Sale {
  clientId: number | null;
  // subTotal.
  sumTotValVenta: number;
  // IGV(18%).
  sumTotTributos: number;
  // importe total.
  sumImpVenta: number;
  // detalle de venta.
  details: Array<SaleDetail>;

  constructor() {
    this.clientId = null;
    this.sumTotValVenta = 0;
    this.sumTotTributos = 0;
    this.sumImpVenta = 0;
    this.details = new Array<SaleDetail>();
  }
}
