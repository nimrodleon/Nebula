import {Warehouse} from './warehouse';

export interface InvoiceSerie {
  id: string;
  name: string;
  warehouseId: string;
  warehouse: Warehouse | any;
  factura: string;
  counterFactura: number;
  boleta: string;
  counterBoleta: number;
  notaDeVenta: string;
  counterNotaDeVenta: number;
}
