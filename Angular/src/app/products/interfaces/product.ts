export class Product {
  id: number | null;
  description: string;
  barcode: string;
  icbper: string;
  price: number;
  igvSunat: string;
  type: string;
  undMedidaId: string;
  pathImage: string;

  constructor() {
    this.id = null;
    this.description = '';
    this.barcode = '';
    this.icbper = '';
    this.price = 0;
    this.igvSunat = '';
    this.type = '';
    this.undMedidaId = '';
    this.pathImage = '';
  }
}
