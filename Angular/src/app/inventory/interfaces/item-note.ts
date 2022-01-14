export class ItemNote {
  constructor(
    public productId: number | any = null,
    public description: string = '',
    public quantity: number = 0,
    public price: number = 0,
    public amount: number = 0) {
  }
}
