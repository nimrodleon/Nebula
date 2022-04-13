export class CashierDetail {
  constructor(
    public id: any = undefined, // identificador clave primaria.
    public cajaDiaria: string = '', // identificador caja diaria.
    public invoiceSale: string = '-', // Clave foránea comprobante de venta.
    public document: string = '', // identificador, serie y número del comprobante.
    public contact: string = '', // identificador y nombre de contacto.
    public remark: string = '', // observación de la operación.
    public typeOperation: 'APERTURA_DE_CAJA' | 'ENTRADA_DE_DINERO' | 'SALIDA_DE_DINERO' | 'COMPROBANTE_DE_VENTA' = 'ENTRADA_DE_DINERO',
    public formaPago: 'Contado:Yape' | 'Credito:Crédito' | 'Contado:Contado' | 'Contado:Depósito' = 'Contado:Contado',
    public amount: number = 0, // Monto de la Operación.
    public hour: string = '') {
  }
}
