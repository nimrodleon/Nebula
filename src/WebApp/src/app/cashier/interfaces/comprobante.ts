export class Comprobante {
  constructor(
    public contactId: string = '', // id contacto.
    public invoiceSale: string = '', // id factura.
    public formaPago: string = '', // forma de pago.
    public docType: 'BOLETA' | 'FACTURA' | 'NOTA' = 'NOTA', // tipo documento.
    public montoRecibido: number = 0, // monto recibido.
    public vuelto: number = 0, // vuelto de la venta.
    public remark: string = '', // observación o comentario.
    public sumTotValVenta: number = 0, // subTotal.
    public sumTotTributos: number = 0, // IGV(18%).
    public sumImpVenta: number = 0, // importe total.
    public sumTotTriIcbper: number = 0) {
  }
}
