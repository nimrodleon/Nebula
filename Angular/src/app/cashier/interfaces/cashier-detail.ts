export class CashierDetail {
  constructor(
    public id: number | any = null, // identificador clave primaria.
    public cajaDiariaId: number | any = null, // identificador de caja diaria.
    public invoiceId: number | any = null, // identificador del comprobante de venta.
    public typeOperation: number = 0, // tipo operación: <Caja Chica/Comprobante>
    public startDate: any = null, // fecha de registro.
    public document: string = '', // serie y número del comprobante.
    public contact: string = '', // nombre de contacto.
    public glosa: string = '', // observación de la operación.
    public paymentMethod: number = 0, // medios de pago: <dinero en efectivo, depósito en cuenta, billetera digital>
    public type: string = 'ENTRADA', // tipo de operación: <ENTRADA/SALIDA>
    public total: number = 0,) {
  }
}
