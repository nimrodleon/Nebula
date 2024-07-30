export class ClaveSolHub {
  user: string = "";
  password: string = "";
}

export class Company {
  id: any = undefined;
  userId: string = "";
  ruc: string = "";
  rznSocial: string = "";
  address: string = "";
  phoneNumber: string = "";
  anchoTicket: string = "";
  codLocalEmisor: string = "";
  tipMoneda: string = "";
  porcentajeIgv: number = 0;
  valorImpuestoBolsa: number = 0;
  contactId: string = "";
  diasPlazo: number = 0;
  ubigueo: string = "";
  departamento: string = "";
  provincia: string = "";
  distrito: string = "";
  urbanizacion: string = "";
  fechaVencimientoCert: string = "-";
  sunatEndpoint: string = "FE_BETA";
  claveSol: ClaveSolHub = new ClaveSolHub();
  modComprobantes: boolean = false;
  modCuentaPorCobrar: boolean = false;
  modReparaciones: boolean = false;
  modCajasDiaria: boolean = false;
  pagoSuscripcionId: string = "";

  public getCompanyTitle(): string {
    return `${this.ruc} - ${this.rznSocial}`;
  }
}

export class LocalSummaryDto {
  public id: number = 0;
  public name: string = "";
}
