import {CpeBase} from './cpe-base';

export class Comprobante extends CpeBase {
  constructor(
    public startDate: any = null,
    public formaPago: string = '',
    public typeOperation: string = '',
    public serie: string = '',
    public number: string = '',
    public EndDate: any = null) {
    super();
  }
}
