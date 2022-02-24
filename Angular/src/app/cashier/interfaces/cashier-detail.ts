export class CashierDetail {
  constructor(
    public id: any = undefined, // identificador clave primaria.
    public cajaDiaria: string = '', // identificador caja diaria.
    public document: string = '', // identificador serie y número del comprobante.
    public contact: string = '', // identificador y nombre de contacto.
    public remark: string = '', // observación de la operación.
    public type: string = 'ENTRADA', // tipo de operación: <ENTRADA/SALIDA>
    public typeOperation: number = 0, // tipo operación: <Caja Chica/Comprobante>
    public formaPago: string = 'Contado', // forma de pago: <Credito|Contado>
    public amount: number = 0, // Monto de la Operación.
    public hour: string = '') {
  }
}
