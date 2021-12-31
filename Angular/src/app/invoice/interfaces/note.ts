import {CpeBase} from './cpe-base';

export class Note extends CpeBase {
  constructor(
    public startDate: any = null,
    public codMotivo: string = '01',
    public serie: string = '',
    public number: string = '',
    public desMotivo: string = '',) {
    super();
  }
}
