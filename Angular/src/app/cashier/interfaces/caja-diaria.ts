export class CajaDiaria {
  id: number | null;
  cajaId: string;
  name: string;
  startDate: any;
  state: string;
  totalApertura: number;
  totalContabilizado: number;
  totalCierre: number;
  year: string;
  month: string;

  constructor() {
    this.id = null;
    this.cajaId = '';
    this.name = '';
    this.state = '';
    this.totalApertura = 0;
    this.totalContabilizado = 0;
    this.totalCierre = 0;
    this.year = '';
    this.month = '';
  }
}
