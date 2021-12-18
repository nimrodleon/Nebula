export interface Configuration {
  id: number;
  ruc: string;
  rznSocial: string;
  codLocalEmisor: string;
  tipMoneda: string;
  porcentajeIgv: number;
  valorImpuestoBolsa: number;
  cpeSunat: string;
  cuentaBancoDetraccion: string;
  textoDetraccion: string;
  montoDetraccion: number;
  contactId: number;
  urlApi: string;
  fileSunat: string;
  fileControl: string;
}
