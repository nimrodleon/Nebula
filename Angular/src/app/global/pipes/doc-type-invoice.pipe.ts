import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
  name: 'docTypeInvoice'
})
export class DocTypeInvoicePipe implements PipeTransform {

  transform(value: string, ...args: unknown[]): unknown {
    let result: string = '';
    switch (value) {
      case 'FT':
        result = 'FACTURA';
        break;
      case 'BL':
        result = 'BOLETA';
        break;
      case 'NV':
        result = 'NOTA DE VENTA';
        break;
    }
    return result;
  }

}
