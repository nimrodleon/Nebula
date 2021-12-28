import {CpeDetail} from './cpe-detail';

/**
 * Clase base para emitir comprobantes electrónicos.
 */
export class CpeBase {
  public ICBPER: number = 0;

  constructor(
    public contactId: number | null = null,
    public docType: string = '', // tipo documento.
    public sumTotValVenta: number = 0, // subTotal.
    public sumTotTributos: number = 0, // IGV(18%).
    public sumImpVenta: number = 0, // importe total.
    public remark: string = '', // observación.
    public details: Array<CpeDetail> = new Array<CpeDetail>(),
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
