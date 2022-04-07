import * as _ from 'lodash';
import {Comprobante} from './comprobante';
import {DetalleComprobante} from './detalle-comprobante';
import {Configuration} from '../../system/interfaces';
import {Product} from '../../products/interfaces';

export class GenerarVenta {
  constructor(
    public comprobante: Comprobante = new Comprobante(),
    public detallesComprobante: Array<DetalleComprobante> = new Array<DetalleComprobante>(),
    public configuration: Configuration = new Configuration()) {
  }

  public agregarProducto(producto: Product): void {
    let _detalleComprobante: DetalleComprobante = new DetalleComprobante();
    const itemVenta = _.find(this.detallesComprobante, (o: DetalleComprobante) => o.productId === producto.id);
    _detalleComprobante.productId = producto.id;
    _detalleComprobante.codUnidadMedida = producto.undMedida;
    _detalleComprobante.codProductoSunat = producto.barcode;
    _detalleComprobante.description = producto.description;
    _detalleComprobante.price = producto.price1;
    _detalleComprobante.igvSunat = producto.igvSunat;
    _detalleComprobante.valorIgv = this.configuration.porcentajeIgv;
    _detalleComprobante.porcentajeIGV = producto.igvSunat === 'GRAVADO' ? (this.configuration.porcentajeIgv / 100) + 1 : 1;
    _detalleComprobante.triIcbper = producto.icbper === 'SI';
    _detalleComprobante.valorIcbper = this.configuration.valorImpuestoBolsa;
    if (!itemVenta) {
      _detalleComprobante.quantity = 1;
      _detalleComprobante = GenerarVenta.calcularPrecioVentaUnitario(_detalleComprobante);
      _detalleComprobante = GenerarVenta.calcularTributoIGV(_detalleComprobante);
      _detalleComprobante = GenerarVenta.calcularTributoICBPER(_detalleComprobante);
      this.detallesComprobante = this.detallesComprobante.concat(_detalleComprobante);
      this.calcularImporteVenta();
    } else {
      _detalleComprobante.quantity = itemVenta.quantity + 1;
      _detalleComprobante = GenerarVenta.calcularPrecioVentaUnitario(_detalleComprobante);
      _detalleComprobante = GenerarVenta.calcularTributoIGV(_detalleComprobante);
      _detalleComprobante = GenerarVenta.calcularTributoICBPER(_detalleComprobante);
      this.detallesComprobante = this.detallesComprobante.map((item: DetalleComprobante) => {
        if (item.productId === producto.id) item = _detalleComprobante;
        return item;
      });
      this.calcularImporteVenta();
    }
  }

  public cambiarCantidad(productId: string, valor: number): void {
    this.detallesComprobante = this.detallesComprobante.map((item: DetalleComprobante) => {
      if (item.productId === productId) {
        item.quantity = Number(valor);
        item = GenerarVenta.calcularPrecioVentaUnitario(item);
        item = GenerarVenta.calcularTributoIGV(item);
        item = GenerarVenta.calcularTributoICBPER(item);
      }
      return item;
    });
    this.calcularImporteVenta();
  }

  public cambiarPrecio(productId: string, valor: number): void {
    this.detallesComprobante = this.detallesComprobante.map((item: DetalleComprobante) => {
      if (item.productId === productId) {
        item.price = Number(valor);
        item = GenerarVenta.calcularPrecioVentaUnitario(item);
        item = GenerarVenta.calcularTributoIGV(item);
        item = GenerarVenta.calcularTributoICBPER(item);
      }
      return item;
    });
    this.calcularImporteVenta();
  }

  public calcularImporteVenta(): void {
    this.comprobante.sumTotValVenta = 0;
    this.comprobante.sumTotTributos = 0;
    this.comprobante.sumTotTriIcbper = 0;
    _.forEach(this.detallesComprobante, (item: DetalleComprobante) => {
      this.comprobante.sumTotValVenta += item.mtoBaseIgvItem;
      this.comprobante.sumTotTributos += item.mtoIgvItem;
      this.comprobante.sumTotTriIcbper += item.mtoTriIcbperItem;
    });
    const {sumTotValVenta, sumTotTributos, sumTotTriIcbper} = this.comprobante;
    this.comprobante.sumImpVenta = sumTotValVenta + sumTotTributos + sumTotTriIcbper;
  }

  public borrarItemVenta(productId: string): void {
    this.detallesComprobante = _.filter(this.detallesComprobante, (o: DetalleComprobante) => o.productId !== productId);
    this.calcularImporteVenta();
  }

  public borrarVenta(): void {
    this.comprobante = new Comprobante();
    this.detallesComprobante = new Array<DetalleComprobante>();
  }

  private static calcularPrecioVentaUnitario(detalleComprobante: DetalleComprobante): DetalleComprobante {
    detalleComprobante.amount = detalleComprobante.price * detalleComprobante.quantity;
    return detalleComprobante;
  }

  private static calcularTributoIGV(detalleComprobante: DetalleComprobante): DetalleComprobante {
    detalleComprobante.mtoBaseIgvItem = detalleComprobante.amount / detalleComprobante.porcentajeIGV;
    detalleComprobante.mtoIgvItem = detalleComprobante.amount - detalleComprobante.mtoBaseIgvItem;
    return detalleComprobante;
  }

  private static calcularTributoICBPER(detalleComprobante: DetalleComprobante): DetalleComprobante {
    if (detalleComprobante.triIcbper) {
      detalleComprobante.mtoTriIcbperItem = detalleComprobante.quantity * detalleComprobante.valorIcbper;
    }
    return detalleComprobante;
  }

}
