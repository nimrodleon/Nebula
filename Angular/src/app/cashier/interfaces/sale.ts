import {SaleDetail} from './sale-detail';

export class Sale {
  clientId: number | null;
  sumTotTributos: number;
  sumTotValVenta: number;
  sumPrecioVenta: number;
  sumDescTotal: number;
  sumImpVenta: number;
  details: Array<SaleDetail>;

  constructor() {
    this.clientId = null;
    this.sumTotTributos = 0;
    this.sumTotValVenta = 0;
    this.sumPrecioVenta = 0;
    this.sumDescTotal = 0;
    this.sumImpVenta = 0;
    this.details = new Array<SaleDetail>();
  }
}
