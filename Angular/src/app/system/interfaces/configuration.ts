export class Configuration {
  constructor(
    public id: number | any = null,
    public ruc: string = '',
    public rznSocial: string = '',
    public codLocalEmisor: string = '',
    public tipMoneda: string = '',
    public porcentajeIgv: number = 0,
    public valorImpuestoBolsa: number = 0,
    public cpeSunat: string = '',
    public cuentaBancoDetraccion: string = '',
    public textoDetraccion: string = '',
    public montoDetraccion: number = 0,
    public contactId: number = 0,
    public urlApi: string = '',
    public fileSunat: string = '',
    public fileControl: string = '') {
  }
}
