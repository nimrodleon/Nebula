import {SaleDetail} from './sale-detail';
import {Cuota} from './cuota';

export class Sale {
  constructor(
    public clientId: number | null = null,
    public paymentType: string = '', // forma de pago.
    public docType: string = '', // tipo documento.
    public endDate: string | undefined = '', // fecha vencimiento.
    public montoCaja: boolean = true,
    public montoTotal: number = 0,
    public vuelto: number = 0,
    public sumTotValVenta: number = 0, // subTotal.
    public sumTotTributos: number = 0, // IGV(18%).
    public sumImpVenta: number = 0, // importe total.
    public remark: string = '', // observación.
    public details: Array<SaleDetail> = new Array<SaleDetail>(), // detalle de venta.
    public cuotas: Array<Cuota> = new Array<Cuota>(),// lista de cuotas a crédito.
  ) {
  }
}
