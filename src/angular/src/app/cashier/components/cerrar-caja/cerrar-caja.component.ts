import {Component, EventEmitter, Input, OnInit, Output} from "@angular/core";
import {FormBuilder, FormGroup, ReactiveFormsModule} from "@angular/forms";
import {CajaDiariaService} from "../../services";
import {CajaDiaria, CerrarCaja} from "../../interfaces";

@Component({
  selector: "app-cerrar-caja",
  standalone: true,
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: "./cerrar-caja.component.html"
})
export class CerrarCajaComponent implements OnInit {
  @Input()
  cajaDiariaId: string = "";
  @Output()
  responseData = new EventEmitter<CajaDiaria>();
  cerrarCajaForm: FormGroup = this.fb.group({
    totalCierre: [0],
  });
  cerrarCajaData: CerrarCaja = {
    totalCierre: 0
  };

  constructor(
    private fb: FormBuilder,
    private cajaDiariaService: CajaDiariaService) {
  }

  ngOnInit(): void {
    // suscribir cambios del formulario.
    this.cerrarCajaForm.valueChanges
      .subscribe(value => this.cerrarCajaData = value);
  }

  // guardar cambios.
  public saveChanges(): void {
    this.cajaDiariaService.update(<any>this.cajaDiariaId, this.cerrarCajaData)
      .subscribe(result => this.responseData.emit(result));
  }

}
