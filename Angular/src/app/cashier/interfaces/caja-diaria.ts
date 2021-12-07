export interface CajaDiaria {
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
}
