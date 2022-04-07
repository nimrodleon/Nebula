export class CajaDiaria {
  constructor(
    public id: any = undefined,
    public terminal: string = '', // identificador y nombre - Serie de facturación.
    public status: string = '', // Estado de Caja.
    public totalApertura: number = 0, // Monto Apertura.
    public totalContabilizado: number = 0, // Monto Contabilizado durante el dia.
    public totalCierre: number = 0, // Monto para el dia siguiente.
    public turno: string = '', // turno del trabajador.
    public createdAt: any = undefined) {
  }
}
