import {CpeBase} from '../../invoice/interfaces';

export class Sale extends CpeBase {
  constructor(
    public paymentMethod: number = 0, // forma de pago.
    public montoTotal: number = 0,
    public vuelto: number = 0,) {
    super();
  }
}
