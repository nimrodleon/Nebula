import { Notyf } from "notyf";
import { Contact } from "app/contact/interfaces";
import { InvoiceSerie } from "../../account/company/interfaces";

// Generador de secuencia (rango).
// Genera un rango de números entre 0..4
// range(0, 4, 1);
// [0, 1, 2, 3, 4]
export const range = (start: number, stop: number, step: number) =>
  Array.from({ length: (stop - start) / step + 1 },
    (_, i) => start + (i * step));

export class TipoComprobanteModalDto {
  constructor(
    public tipoComprobante: string = "",
    public invoiceSerieId: string = "") {
  }
}

export class TipoComprobanteDataModal {
  constructor(
    public type: "LIST" | "DETAIL" = "LIST",
    public contact: Contact = new Contact(),
    public invoiceSeries: Array<InvoiceSerie> = new Array<InvoiceSerie>()) {
  }
}

export enum FormType { ADD = "ADD", EDIT = "EDIT" }

// tipo de emisión electrónica.
export enum EnumTypeSee { NONE = "NONE", SFS = "SFS", NRUS = "NRUS" }

// Código de tipo de documento de identidad
export enum EnumTypeDocUsuario {
  SIN_DEFINIR = "0:SIN DEFINIR",
  DNI = "1:D.N.I",
  RUC = "6:R.U.C",
  PASAPORTE = "7:PASAPORTE"
}

export enum EnumIdModal {
  //#region CONTACT
  ID_CONTACT_REGISTER_MODAL = "#contact-modal",
  ID_SEARCH_CONTACT_MODAL = "#searchContactModal",
  //#endregion
  //#region SALES
  ID_ADD_ENTRADA_MANUAL_SALES = "#addEntradaModal",
  ID_ADD_PRODUCT_MODAL_SALES = "#addProductModalSales",
  CONSULTAR_VALIDEZ_DIARIA = "#consultarValidezDiaria",
  //#endregion
  //#region PURCHASES
  ID_ADD_PRODUCT_MODAL_PURCHASES = "#addProductModalPurchases",
  //#endregion
}

export function getIgvSunat(codTriIgv: string) {
  let result = "";
  if (codTriIgv === "1000") result = "GRAVADO";
  if (codTriIgv === "9997") result = "EXONERADO";
  if (codTriIgv === "9998") result = "INAFECTO";
  return result;
}

export function isValidObjectId(objectId: string): boolean {
  const objectIdPattern = /^[0-9a-fA-F]{24}$/;
  return objectIdPattern.test(objectId);
}

const notyf: Notyf = new Notyf();

export function toastSuccess(message: string) {
  notyf.success(message);
}

export function toastError(message: string) {
  notyf.error(message);
}

export class PaginationLink {
  page: number = 1;
}

export class PaginationInfo {
  currentPage: number = 0;
  totalPages: number = 0;
  previousPage: number | null = null;
  pages: PaginationLink[] = [];
  nextPage: number | null = null;
}

export class PaginationResult<T> {
  pagination: PaginationInfo = new PaginationInfo();
  data: T[] = [];
}
