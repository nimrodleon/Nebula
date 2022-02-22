export class Product {
  constructor(
    public id: number | any = null,
    public description: string = '',
    public barcode: string = '',
    public price1: number = 0,
    public price2: number = 0,
    public fromQty: number = 0,
    public igvSunat: string = '',
    public icbper: string = '',
    public categoryId: number | any = null,
    public undMedida: string = '',
    public type: string = '',
    public pathImage: string = '') {
  }
}
