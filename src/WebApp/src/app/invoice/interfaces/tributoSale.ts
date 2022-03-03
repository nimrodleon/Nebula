export class TributoSale {
  constructor(
    public id: any = undefined,
    public invoiceId: number | any = null, // Clave foránea.
    public ideTributo: string = '', // Identificador de tributo.
    public nomTributo: string = '', // Nombre de tributo.
    public codTipTributo: string = '', // Código de tipo de tributo.
    public mtoBaseImponible: number = 0, // Base imponible.
    public mtoTributo: number = 0) {
  }
}
