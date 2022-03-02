export class InvoiceAccount {
  constructor(
    public id: string | any = null,
    public invoiceId: number | any = null, // Clave foránea.
    public serie: string = '', // Serie comprobante.
    public number: string = '', // Número comprobante.
    public accountType: string = '', // Tipo factura (Cobrar|Pagar).
    public status: string = '', // Estado Cuenta (PENDIENTE|COBRADO|ANULADO).
    public cuota: number = 0, // Número de Cuota.
    public amount: number = 0, // Monto Cuenta.
    public balance: number = 0, // Saldo de la Cuenta.
    public endDate: any = null,) {
  }
}
