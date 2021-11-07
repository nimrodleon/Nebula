import {SaleDetail} from './sale-detail';
import {Cuota} from './cuota';

export class Sale {
  clientId: number | null;
  // forma de pago.
  paymentType: string;
  // tipo documento.
  docType: string;
  // fecha vencimiento.
  endDate: string;
  // monto caja.
  montoCaja: boolean;
  // monto entregado.
  montoTotal: number;
  // vuelto entregado.
  vuelto: number;
  // subTotal.
  sumTotValVenta: number;
  // IGV(18%).
  sumTotTributos: number;
  // importe total.
  sumImpVenta: number;
  // observación.
  remark: string;
  // detalle de venta.
  details: Array<SaleDetail>;
  // lista de cuotas a crédito.
  cuotas: Array<Cuota>;

  constructor() {
    this.clientId = null;
    this.paymentType = '';
    this.docType = '';
    this.endDate = '';
    this.montoCaja = true;
    this.montoTotal = 0;
    this.vuelto = 0;
    this.sumTotValVenta = 0;
    this.sumTotTributos = 0;
    this.sumImpVenta = 0;
    this.remark = '';
    this.details = new Array<SaleDetail>();
    this.cuotas = new Array<Cuota>();
  }
}
