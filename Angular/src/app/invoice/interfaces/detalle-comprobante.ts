export class DetalleComprobante {
  productId: number | null;
  description: string;
  price: number;
  quantity: number;
  amount: number;

  constructor() {
    this.productId = null;
    this.description = '';
    this.price = 0;
    this.quantity = 0;
    this.amount = 0;
  }
}
