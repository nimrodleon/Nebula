import {CpeDetail} from './cpe-detail';
import {Configuration} from '../../system/interfaces';
import {Product} from '../../products/interfaces';
import {InvoiceDetail} from './invoice-detail';

// modelo genérico comprobante.
// usado solo para el calculo del detalle del comprobante y los subTotales.
export class CpeGeneric {
  public ICBPER: number = 0;

  constructor(
    public sumTotValVenta: number = 0, // subTotal.
    public sumTotTributos: number = 0, // IGV(18%).
    public sumImpVenta: number = 0, // importe total.
    public details: Array<CpeDetail> = new Array<CpeDetail>()) {
  }

  // calcular importe de venta.
  public calcImporteVenta(): void {
    this.ICBPER = 0;
    this.sumTotValVenta = 0;
    this.sumTotTributos = 0;
    this.details.forEach(item => {
      this.sumTotValVenta = this.sumTotValVenta + item.mtoBaseIgvItem;
      this.sumTotTributos = this.sumTotTributos + item.mtoIgvItem;
      this.ICBPER = this.ICBPER + item.mtoTriIcbperItem;
    });
    this.sumImpVenta = this.sumTotValVenta + this.sumTotTributos + this.ICBPER;
  }

  // agregar item al detalle de venta.
  public addItemDetail(configuration: Configuration, product: Product): void {
    if (this.details.length <= 0) {
      this.details.push(CpeGeneric.configItemDetail(configuration, product));
      this.calcImporteVenta();
    } else {
      let changeQuantity: boolean = false;
      this.details.forEach((item: CpeDetail) => {
        if (item.productId === product.id) {
          item.quantity = item.quantity + 1;
          item.price = product.price1;
          item.calcularItem();
          this.calcImporteVenta();
          changeQuantity = true;
        }
      });
      // ejecutar si no hay coincidencias.
      if (!changeQuantity) {
        this.details.push(CpeGeneric.configItemDetail(configuration, product));
        this.calcImporteVenta();
      }
    }
  }

  // agregar item al detalle de venta.
  public addItemWithData(cpeDetail: CpeDetail): void {
    if (this.details.length <= 0) {
      this.details.push(cpeDetail);
      this.calcImporteVenta();
    } else {
      this.deleteItem(cpeDetail.productId);
      this.details.push(cpeDetail);
      this.calcImporteVenta();
    }
  }

  // cambiar cantidad item.
  public changeQuantity(prodId: number, value: number): void {
    this.details.forEach((item: CpeDetail) => {
      if (item.productId === prodId) {
        item.quantity = value;
        item.calcularItem();
        this.calcImporteVenta();
      }
    });
  }

  // borrar item detalle comprobante.
  public deleteItem(prodId: number | any): void {
    this.details.forEach((value: CpeDetail, index: number, array: CpeDetail[]) => {
      if (value.productId === prodId) {
        array.splice(index, 1);
        this.calcImporteVenta();
      }
    });
  }

  // configurar item detalle pasando producto y configuración del sistema.
  public static configItemDetail(configuration: Configuration, product: Product): CpeDetail {
    const item: CpeDetail = new CpeDetail();
    item.productId = Number(product.id);
    item.codUnidadMedida = product.undMedida.sunatCode;
    item.codProductoSunat = product.barcode.length > 0 ? product.barcode : '-';
    item.description = product.description;
    item.price = product.price1;
    item.quantity = 1;
    item.igvSunat = product.igvSunat;
    item.valorIgv = configuration.porcentajeIgv;
    item.triIcbper = product.icbper === 'SI';
    item.valorIcbper = configuration.valorImpuestoBolsa;
    item.porcentajeIGV = configuration.porcentajeIgv;
    item.mtoTriIcbperUnidad = configuration.valorImpuestoBolsa;
    return item;
  }

  // configurar item detalle desde una factura.
  public static getItemDetail(invoiceDetail: InvoiceDetail): CpeDetail {
    const item: CpeDetail = new CpeDetail();
    item.productId = Number(invoiceDetail.codProducto);
    item.codUnidadMedida = invoiceDetail.codUnidadMedida;
    item.codProductoSunat = invoiceDetail.codProductoSunat;
    item.description = invoiceDetail.desItem;
    item.price = invoiceDetail.mtoPrecioVentaUnitario;
    item.quantity = invoiceDetail.ctdUnidadItem;
    if (invoiceDetail.codTriIgv === '1000') item.igvSunat = 'GRAVADO';
    if (invoiceDetail.codTriIgv === '9997') item.igvSunat = 'EXONERADO';
    if (invoiceDetail.codTriIgv === '9996') item.igvSunat = 'GRATUITO';
    item.valorIgv = Number(invoiceDetail.porIgvItem);
    item.triIcbper = invoiceDetail.codTriIcbper !== '-';
    item.valorIcbper = Number(invoiceDetail.mtoTriIcbperUnidad);
    item.porcentajeIGV = Number(invoiceDetail.porIgvItem);
    item.mtoTriIcbperUnidad = Number(invoiceDetail.mtoTriIcbperUnidad);
    return item;
  }
}
