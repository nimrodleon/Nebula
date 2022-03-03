import {CpeBase} from 'src/app/invoice/interfaces';

// modelo para el punto de venta.
export class Sale extends CpeBase {
  constructor(
    public formaPago: string = '',
    public montoTotal: number = 0,
    public vuelto: number = 0,) {
    super();
  }
}
