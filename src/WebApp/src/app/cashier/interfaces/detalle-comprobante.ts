export class DetalleComprobante {
  constructor(
    public productId: string = '', // Enviar.
    public tipoItem: string = '', // Tipo de registro.
    public codUnidadMedida: string = '', // Enviar.
    public codProductoSunat: string = '', // Enviar.
    public description: string = '', // Enviar.
    public price: number = 0, // Enviar.
    public quantity: number = 0, // Enviar.
    public igvSunat: string = 'GRAVADO', // Enviar.
    public valorIgv: number = 0, // Enviar.
    public mtoIgvItem: number = 0,
    public mtoBaseIgvItem: number = 0,
    public amount: number = 0,
    public triIcbper: boolean = false, // Enviar.
    public valorIcbper: number = 0, // Enviar.
    public mtoTriIcbperItem: number = 0,
    public porcentajeIGV: number = 0) {
  }
}
