export class Comprobante {
  constructor(
    public contactId: string = '', // id contacto.
    public sumTotValVenta: number = 0, // subTotal.
    public sumTotTributos: number = 0, // IGV(18%).
    public sumImpVenta: number = 0, // importe total.
    public sumTotTriIcbper: number = 0) {
  }
}
