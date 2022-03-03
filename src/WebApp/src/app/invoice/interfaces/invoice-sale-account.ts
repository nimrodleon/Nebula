export class InvoiceSaleAccount {
  constructor(
    public id: any = undefined,
    public invoiceSale: string = '', // Clave foránea.
    public serie: string = '', // Serie comprobante.
    public number: string = '', // Número comprobante.
    public status: string = '', // Estado Cuenta (PENDIENTE|COBRADO|ANULADO).
    public cuota: number = 0, // Número de Cuota.
    public amount: number = 0, // Monto Cuenta.
    public balance: number = 0, // Saldo de la Cuenta.
    public endDate: any = null) {
  }
}
