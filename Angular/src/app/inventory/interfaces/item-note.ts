export class ItemNote {
  constructor(
    public productId: number | any = null, // Id del producto.
    public description: string = '', // Descripción del Item o nombre del producto.
    public quantity: number = 0, // Cantidad del Item.
    public price: number = 0, // Precio del Producto/Servicio.
    public amount: number = 0 /* Monto del Item (cantidad * precio). */) {
  }
}
