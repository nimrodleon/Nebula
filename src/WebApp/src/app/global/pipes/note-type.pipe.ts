import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
  name: 'noteType'
})
export class NoteTypePipe implements PipeTransform {
  transform(value: string): string {
    let result: string = '';
    switch (value) {
      case 'NC':
        result = 'Nota de Crédito';
        break;
      case 'ND':
        result = 'Nota de Débito';
        break;
    }
    return result;
  }
}
