import {Component, EventEmitter, Output} from "@angular/core";
import {faLock} from "@fortawesome/free-solid-svg-icons";
import {ProductService} from "../../services";
import {catchError} from "rxjs/operators";

@Component({
  selector: "app-carga-masiva-productos",
  standalone: true,
  templateUrl: "./carga-masiva-productos.component.html"
})
export class CargaMasivaProductosComponent {
  faLock = faLock;
  private archivoExcel: any;
  @Output()
  loading: EventEmitter<boolean> = new EventEmitter<boolean>(false);
  @Output()
  success: EventEmitter<boolean> = new EventEmitter<boolean>(false);

  constructor(private productService: ProductService) {
  }

  public descargarPlantilla(event: Event): void {
    event.preventDefault();
    this.productService.descargarPlantilla()
      .subscribe(data => {
        const blob: Blob = new Blob([data], {type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"});
        const url: string = window.URL.createObjectURL(blob);
        const a: HTMLAnchorElement = document.createElement("a");
        a.href = url;
        a.download = "plantilla.xlsx";
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
      });
  }

  public selectedExcel(event: any): void {
    this.archivoExcel = event.target.files[0];
  }

  public cargarArchivo(): void {
    this.loading.emit(true);
    const formData = new FormData();
    formData.append("datos", this.archivoExcel);
    this.productService.cargarProductos(formData)
      .pipe(catchError(err => {
        this.loading.emit(false);
        this.success.emit(false);
        throw err;
      }))
      .subscribe(() => {
        this.loading.emit(false);
        this.success.emit(true);
      });
  }

}
