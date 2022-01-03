import {Warehouse} from './warehouse';

export class InvoiceSerie {
  constructor(
    public id: number | any = null,
    public name: string = '',
    public warehouseId: number | any = null,
    public warehouse: Warehouse | any = undefined,
    public factura: string = '',
    public counterFactura: number = 0,
    public boleta: string = '',
    public counterBoleta: number = 0,
    public notaDeVenta: string = '',
    public counterNotaDeVenta: number = 0) {
  }
}
