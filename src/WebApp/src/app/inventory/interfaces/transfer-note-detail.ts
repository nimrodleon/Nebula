export class TransferNoteDetail {
  constructor(
    public id: string = '',
    public transferNoteId: number = 0, // Id Nota de Transferencia.
    public productId: number = 0, // Id del producto.
    public description: string = '', // Descripción del producto.
    public price: number = 0, // Precio del producto.
    public quantity: number = 0, // Cantidad del producto.
    public amount: number = 0) {
  }
}
