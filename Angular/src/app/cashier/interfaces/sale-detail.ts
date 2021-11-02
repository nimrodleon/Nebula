export class SaleDetail {
  productId: number | null;
  description: string;
  price: number;
  quantity: number;
  discount: number;
  amount: number;

  constructor(
    productId: number | null,
    description: string,
    price: number = 0, quantity: number = 0,
    discount: number = 0, amount: number = 0) {
    this.productId = productId;
    this.description = description;
    this.price = price;
    this.quantity = quantity;
    this.discount = discount;
    this.amount = amount;
  }
}
