import {SaleDetail} from './sale-detail';

export class Sale {
  public ICBPER: number = 0;

  constructor(
    public clientId: number | null = null,
    public paymentMethod: number = 0, // forma de pago.
    public docType: string = '', // tipo documento.
    public montoTotal: number = 0,
    public vuelto: number = 0,
    public sumTotValVenta: number = 0, // subTotal.
    public sumTotTributos: number = 0, // IGV(18%).
    public sumImpVenta: number = 0, // importe total.
    public remark: string = '', // observación.
    public details: Array<SaleDetail> = new Array<SaleDetail>(),
    public invoiceId: number | any = undefined) {
  }

  // calcular importe de venta.
  public calcImporteVenta(): void {
    this.ICBPER = 0;
    this.sumTotValVenta = 0;
    this.sumTotTributos = 0;
    this.details.forEach(item => {
      this.sumTotValVenta = this.sumTotValVenta + item.mtoBaseIgvItem;
      this.sumTotTributos = this.sumTotTributos + item.mtoIgvItem;
      this.ICBPER = this.ICBPER + item.mtoTriIcbperItem;
    });
    this.sumImpVenta = this.sumTotValVenta + this.sumTotTributos + this.ICBPER;
  }

}
