export interface Product {
  id: number | null;
  description: string;
  barcode: string;
  price1: number;
  price2: number;
  fromQty: number;
  igvSunat: string;
  icbper: string;
  categoryId: string;
  undMedidaId: string;
  undMedida: any;
  type: string;
  pathImage: string;
}
