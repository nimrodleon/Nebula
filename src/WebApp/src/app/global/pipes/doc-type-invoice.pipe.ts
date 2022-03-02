import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
  name: 'docTypeInvoice'
})
export class DocTypeInvoicePipe implements PipeTransform {

  transform(value: string): string {
    let result: string = '';
    switch (value) {
      case 'FACTURA':
        result = 'FACTURA';
        break;
      case 'BOLETA':
        result = 'BOLETA';
        break;
      case 'NOTA':
        result = 'NOTA DE VENTA';
        break;
    }
    return result;
  }

}
