import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
  name: 'formaPago'
})
export class FormaPagoPipe implements PipeTransform {

  transform(value: string): string {
    let result: string = '';
    switch (value) {
      case 'Credito':
        result = 'CRÉDITO';
        break;
      case 'Contado':
        result = 'CONTADO';
        break;
    }
    return result;
  }

}
