export class SaleDetail {
  constructor(
    public productId: number | null = null,
    public description: string = '',
    public quantity: number = 0,
    public price: number = 0,
    public amount: number = 0) {
  }
}
