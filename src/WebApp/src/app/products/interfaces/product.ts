export class Product {
  constructor(
    public id: any = undefined,
    public description: string = '',
    public barcode: string = '',
    public price1: number = 0,
    public price2: number = 0,
    public fromQty: number = 0,
    public igvSunat: string = '',
    public icbper: string = '',
    public category: string = '',
    public undMedida: string = '',
    public type: string = '',
    public pathImage: string = '') {
  }
}
