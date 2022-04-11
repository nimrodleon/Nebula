import {Comprobante} from './comprobante';

export class ResponseCobrarModal {
  constructor(
    public data: Comprobante = new Comprobante(),
    public status: 'HIDE' | 'COMPLETE' | 'PRINT' = 'HIDE') {
  }
}
