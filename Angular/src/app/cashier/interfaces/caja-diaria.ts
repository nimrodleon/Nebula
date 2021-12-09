export interface CajaDiaria {
  id: number | null;
  invoiceSerieId: string;
  name: string;
  startDate: any;
  status: string;
  totalApertura: number;
  totalContabilizado: number;
  totalCierre: number;
  year: string;
  month: string;
}
