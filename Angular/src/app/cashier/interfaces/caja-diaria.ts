export class CajaDiaria {
  constructor(
    public id: number | any = null,
    public invoiceSerieId: string = '', // Clave foránea Serie de facturación.
    public name: string = '', // Nombre identificador de Caja.
    public startDate: any = null, // Fecha de Operación.
    public status: string = '', // Estado Caja.
    public totalApertura: number = 0, // Monto Apertura.
    public totalContabilizado: number = 0, // Monto Contabilizado durante el dia.
    public totalCierre: number = 0, /* Monto para el dia siguiente. */) {
  }
}
