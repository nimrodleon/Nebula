import moment from "moment";

export type TypeReceivable = "CARGO" | "ABONO";
export type StatusReceivable = "PENDIENTE" | "COBRADO" | "-";

export class Receivable {
  constructor(
    public id: string = "-",
    public companyId: string = "",
    public type: TypeReceivable = "CARGO",
    public contactId: string = "",
    public contactName: string = "",
    public remark: string = "",
    public invoiceSale: string = "-",
    public docType: string = "-",
    public document: string = "-",
    public formaPago: string = "-",
    public cargo: number = 0,
    public abono: number = 0,
    public saldo: number = 0,
    public status: StatusReceivable = "PENDIENTE",
    public cajaDiaria: string = "-",
    public terminal: string = "-",
    public receivableId: string = "-",
    public createdAt: string = moment().format("YYYY-MM-DD"),
    public endDate: string = "",
    public year: string = moment().format("YYYY"),
    public month: string = moment().format("MM")) {
  }
}

export class ReceivableDataModal {
  constructor(
    public title: string = "",
    public type: "ADD" | "EDIT" = "ADD",
    public receivable: Receivable = new Receivable()) {
  }
}

export class ReceivableDetailDataModal {
  constructor(
    public cargo: Receivable = new Receivable(),
    public abonos: Array<Receivable> = new Array<Receivable>()) {
  }
}

export class ReceivableRequestParams {
  constructor(
    public year: string = "",
    public month: string = "",
    public status: string = "-") {
  }
}

export class CuentaPorCobrarParam {
  status: string = "";
  year: string = "";
}

export class CuentaPorCobrarClienteAnualParam extends CuentaPorCobrarParam {
  contactId: string = "";
}

export class CuentaPorCobrarMensualParam extends CuentaPorCobrarParam {
  month: string = "";
}

export class DeudaCliente {
  contactId: string = '';
  contactName: string = '';
  deudaTotal: number = 0;
  receivables: Receivable[] = [];
}

export class ResumenDeuda {
  totalPorCobrar: number = 0;
  deudasPorCliente: DeudaCliente[] = [];
}
