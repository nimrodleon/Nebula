export class CashierDetail {
  id: number | null | undefined;
  cajaDiariaId: number | null;
  startDate: any;
  document: string;
  contact: string;
  glosa: string;
  type: string;
  total: number;

  constructor() {
    this.id = null;
    this.cajaDiariaId = null;
    this.document = '';
    this.contact = '';
    this.glosa = '';
    this.type = '';
    this.total = 0;
  }
}
