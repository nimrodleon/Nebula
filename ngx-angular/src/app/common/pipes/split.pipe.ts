import {Pipe, PipeTransform} from "@angular/core";

@Pipe({
  standalone: true,
  name: "split"
})
export class SplitPipe implements PipeTransform {

  transform(value: string, index?: number): string {
    const arrText: Array<string> = value.split(":");
    let result: string;

    if (index !== undefined && index < arrText.length) {
      result = arrText[index].trim();
    } else {
      // Si no se pasa un índice o el índice está fuera de rango,
      // se utiliza el último elemento del array
      result = arrText[arrText.length - 1].trim();
    }
    return result;
  }

}
