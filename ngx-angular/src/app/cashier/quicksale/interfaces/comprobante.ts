import {Product} from "app/products/interfaces";
import {Company} from "app/account/interfaces";
import {Contact} from "app/contact/interfaces";
import {BillingResponse} from "app/sales/interfaces";
import _ from "lodash";

export class CabeceraComprobanteDto {
  cajaDiariaId: string = "-";
  tipoDoc: string = "NOTA";
  contactId: string = "";
  tipDocUsuario: string = "";
  numDocUsuario: string = "";
  rznSocialUsuario: string = "";
  paymentMethod: string = "";
  fecVencimiento: string = "-";
  invoiceSerieId: string = "-";
  remark: string = "";
}

export class ItemComprobanteDto {
  tipoItem: string = "BIEN";
  ctdUnidadItem: number = 0;
  codUnidadMedida: string = "NIU:UNIDAD (BIENES)";
  description: string = "";
  igvSunat: string = "GRAVADO";
  productId: string = "-";
  mtoPrecioVentaUnitario: number = 0;
  warehouseId: string = "-";
  recordType: "PRODUCTO" | "ENTRADA" = "PRODUCTO";

  public mapProductToItem(product: Product): void {
    this.tipoItem = product.type;
    this.codUnidadMedida = product.undMedida;
    this.description = product.description;
    this.igvSunat = product.igvSunat;
    this.productId = product.id;
    this.mtoPrecioVentaUnitario = product.precioVentaUnitario;
  }

  public getValorVentaItem(company: Company): number {
    const porcentajeIgv = this.igvSunat === "GRAVADO" ?
      (company.porcentajeIgv / 100) + 1 : 1;
    return (this.ctdUnidadItem * this.mtoPrecioVentaUnitario) / porcentajeIgv;
  }

  public getMtoIgvItem(company: Company): number {
    const porcentajeIgv = this.igvSunat === "GRAVADO" ?
      (company.porcentajeIgv / 100) + 1 : 1;
    const valorVentaItem = (this.ctdUnidadItem * this.mtoPrecioVentaUnitario) / porcentajeIgv;
    const importeTotal = this.ctdUnidadItem * this.mtoPrecioVentaUnitario;
    return importeTotal - valorVentaItem;
  }

  public getImporteTotal(): number {
    return this.ctdUnidadItem * this.mtoPrecioVentaUnitario;
  }
}

export class PaymentTerms {
  public moneda: string = "PEN";
  public tipo: string = "Contado";
  public monto: number = 0;
}

export class Cuota {
  public moneda: string = "PEN";
  public monto: number = 0;
  public fechaPago: string = "";
}

export class ComprobanteDto {
  cabecera: CabeceraComprobanteDto = new CabeceraComprobanteDto();
  detalle: ItemComprobanteDto[] = [];
  formaPago: PaymentTerms = new PaymentTerms();
  cuotas: Cuota[] = [];

  private mapProductToInvoiceItemDto(product: Product): ItemComprobanteDto {
    const itemComprobanteDto = new ItemComprobanteDto();
    itemComprobanteDto.tipoItem = product.type.trim();
    itemComprobanteDto.codUnidadMedida = product.undMedida.trim();
    itemComprobanteDto.description = product.description.trim();
    itemComprobanteDto.igvSunat = product.igvSunat.trim();
    itemComprobanteDto.productId = product.id.trim();
    itemComprobanteDto.mtoPrecioVentaUnitario = product.precioVentaUnitario;
    return itemComprobanteDto;
  }

  private getItemComprobanteDto(productId: string): ItemComprobanteDto | undefined {
    return _.find(this.detalle, item => item.productId === productId);
  }

  private verificarProductoEnLista(productId: string): boolean {
    return _.some(this.detalle, item => item.productId === productId);
  }

  private existeProductoEnDetalle(productId: string, warehouseId: string): boolean {
    return _.some(this.detalle, item => item.productId === productId && item.warehouseId === warehouseId);
  }

  public agregarItemAlDetalle(nuevoItem: ItemComprobanteDto): void {
    const itemComprobanteDto: ItemComprobanteDto = new ItemComprobanteDto();
    itemComprobanteDto.tipoItem = nuevoItem.tipoItem;
    itemComprobanteDto.ctdUnidadItem = nuevoItem.ctdUnidadItem;
    itemComprobanteDto.codUnidadMedida = nuevoItem.codUnidadMedida;
    itemComprobanteDto.description = nuevoItem.description;
    itemComprobanteDto.igvSunat = nuevoItem.igvSunat;
    itemComprobanteDto.productId = nuevoItem.productId;
    itemComprobanteDto.mtoPrecioVentaUnitario = nuevoItem.mtoPrecioVentaUnitario;
    itemComprobanteDto.warehouseId = nuevoItem.warehouseId;
    itemComprobanteDto.recordType = nuevoItem.recordType;
    const {productId, warehouseId} = nuevoItem;
    if (!this.existeProductoEnDetalle(productId, warehouseId)) {
      this.detalle = _.concat(this.detalle, itemComprobanteDto);
    } else {
      this.detalle = _.map(this.detalle, item => {
        if (item.productId === productId && item.warehouseId === warehouseId) item = itemComprobanteDto;
        return item;
      });
    }
  }

  public agregarDetalle(product: Product, warehouseId: string): void {
    const itemComprobanteDto: ItemComprobanteDto = this.mapProductToInvoiceItemDto(product);
    itemComprobanteDto.warehouseId = warehouseId.trim();
    if (this.verificarProductoEnLista(product.id)) {
      const itemComprobanteFromList = this.getItemComprobanteDto(product.id);
      if (itemComprobanteFromList !== undefined) {
        itemComprobanteDto.ctdUnidadItem = itemComprobanteFromList.ctdUnidadItem + 1;
        this.detalle = _.map(this.detalle, item => {
          if (item.productId === itemComprobanteDto.productId)
            item = itemComprobanteDto;
          return item;
        });
      }
    } else {
      itemComprobanteDto.ctdUnidadItem = 1;
      this.detalle = _.concat(this.detalle, itemComprobanteDto);
    }
  }

  public deleteItemByProductAndWarehouse(productId: string, warehouseId: string): void {
    this.detalle = _.filter(this.detalle, item => !(item.productId === productId && item.warehouseId === warehouseId));
  }

  public borrarItemDetalle(productId: string): void {
    this.detalle = _.filter(this.detalle, item => item.productId !== productId);
  }

  public setCliente(contact: Contact): void {
    const cabecera = this.cabecera;
    cabecera.contactId = contact.id.trim();
    cabecera.tipDocUsuario = contact.docType.trim();
    cabecera.numDocUsuario = contact.document.trim();
    cabecera.rznSocialUsuario = contact.name.trim();
    this.cabecera = {...this.cabecera, ...cabecera};
  }

  public getValorVenta(company: Company): number {
    return _.sumBy(this.detalle, item => item.getValorVentaItem(company));
  }

  public getMtoIgv(company: Company): number {
    return _.sumBy(this.detalle, item => item.getMtoIgvItem(company));
  }

  public getTotalCobrar(): number {
    return _.sumBy(this.detalle, item => item.getImporteTotal());
  }

}

export class ResponseCobrarModal {
  constructor(
    public invoiceSaleId: string = "-",
    public billingResponse: BillingResponse = new BillingResponse(),
    public imprimir: boolean = false) {
  }
}
