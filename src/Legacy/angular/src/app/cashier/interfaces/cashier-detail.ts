export class CashierDetail {
  id: string = "";
  companyId: string = "";
  cajaDiariaId: string = "";
  invoiceSaleId: string = "-";
  docType: string = "NOTA";
  document: string = "-";
  contactId: string = "-";
  contactName: string = "-";
  remark: string = "-";
  typeOperation: "APERTURA_DE_CAJA" | "ENTRADA_DE_DINERO" | "SALIDA_DE_DINERO" | "COMPROBANTE_DE_VENTA" = "ENTRADA_DE_DINERO";
  formaPago: "Contado:Yape" | "Credito:Crédito" | "Contado:Contado" | "Contado:Depósito" = "Contado:Contado";
  amount: number = 0;
  hour: string = "";
  createdAt: string = "";
}
