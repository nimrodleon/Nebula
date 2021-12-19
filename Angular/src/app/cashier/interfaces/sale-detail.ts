export class SaleDetail {
  private _porcentajeIGV: number = 0;
  private _mtoTriIcbperUnidad: number = 0;

  constructor(
    public productId: number | null = null, // Enviar.
    public description: string = '', // Enviar.
    public price: number = 0, // Enviar.
    public quantity: number = 0, // Enviar.
    public igvSunat: string = 'GRAVADO', // Enviar.
    public mtoIgvItem: number = 0,
    public mtoBaseIgvItem: number = 0,
    public amount: number = 0,
    public triIcbper: boolean = false, // Enviar.
    public mtoTriIcbperItem: number = 0) {
  }

  public set porcentajeIGV(value: number) {
    // se retorna <1> en el caso que IGV sea distinto a <GRAVADO>.
    this._porcentajeIGV = this.igvSunat === 'GRAVADO' ? (value / 100) + 1 : 1;
  }

  public set mtoTriIcbperUnidad(value: number) {
    this._mtoTriIcbperUnidad = value;
  }

  public calcularItem(): void {
    // Establecer valor de los Tributos primero.
    this.calcularPrecioVentaUnitario();
    this.calcularTributoIGV();
    this.calcularTributoICBPER();
  }

  private calcularPrecioVentaUnitario(): void {
    this.amount = this.price * this.quantity;
  }

  private calcularTributoIGV(): void {
    this.mtoBaseIgvItem = this.amount / this._porcentajeIGV;
    this.mtoIgvItem = this.amount - this.mtoBaseIgvItem;
  }

  private calcularTributoICBPER(): void {
    if (this.triIcbper) {
      this.mtoTriIcbperItem = this.quantity * this._mtoTriIcbperUnidad;
    }
  }

}
