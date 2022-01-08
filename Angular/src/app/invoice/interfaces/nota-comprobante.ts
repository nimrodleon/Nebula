import {CpeGeneric} from './cpe-generic';

// modelo Nota crédito/débito.
export class NotaComprobante extends CpeGeneric {
  constructor(
    public invoiceId: number | any = null, // Id del Comprobante.
    public startDate: any = null, // Fecha de registro.
    public docType: string = '', // Tipo documento NOTA: (CRÉDITO/DÉBITO) => (NC/ND).
    public codMotivo: string = '', // Código del Motivo de emisión.
    public serie: string = '', // Serie Comprobante #SOLO PARA COMPRAS.
    public number: string = '', // Número Comprobante #SOLO PARA COMPRAS.
    public desMotivo: string = '', /* Descripción del motivo. */) {
    super();
  }
}
