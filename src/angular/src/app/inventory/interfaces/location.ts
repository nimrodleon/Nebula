export class Location {
  constructor(
    public id: any = undefined,
    public companyId: string = "",
    public warehouseId: string = "",
    public warehouseName: string = "",
    public description: string = "") {
  }
}

export class LocationDetail {
  constructor(
    public id: any = undefined,
    public locationId: string = "",
    public productId: string = "",
    public productName: string = "",
    public barcode: string = "",
    public quantityMax: number = 0,
    public quantityMin: number = 0) {
  }
}

export class ProductLocationDataModal {
  constructor(
    public title: string = "",
    public type: "ADD" | "EDIT" = "ADD",
    public locationDetail: LocationDetail = new LocationDetail()) {
  }
}

export class LocationItemStockDto {
  constructor(
    public productId: string = "",
    public description: string = "",
    public quantityMax: number = 0,
    public quantityMin: number = 0,
    public stock: number = 0) {
  }
}

export class RespLocationDetailStock {
  constructor(
    public location: Location = new Location(),
    public locationDetailStocks: Array<LocationItemStockDto> = new Array<LocationItemStockDto>()) {
  }
}
