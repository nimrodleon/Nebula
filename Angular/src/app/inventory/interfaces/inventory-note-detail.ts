export class InventoryNoteDetail {
  constructor(
    public id: string = '',
    public inventoryNoteId: number = 0, // Id Nota de Inventario.
    public productId: number = 0, // Id del Producto.
    public description: string = '', // Descripción Item.
    public price: number = 0, // Precio Item.
    public quantity: number = 0, // Cantidad del Item.
    public amount: number = 0 /* Monto del Item (cantidad * precio). */) {
  }
}
