export class Configuration {
  constructor(
    public id: any = undefined,
    public ruc: string = '',
    public rznSocial: string = '',
    public codLocalEmisor: string = '',
    public tipMoneda: string = '',
    public porcentajeIgv: number = 0,
    public valorImpuestoBolsa: number = 0,
    public cpeSunat: string = '',
    public contactId: any = undefined,
    public urlApi: string = '',
    public fileSunat: string = '',
    public fileControl: string = '') {
  }
}
