import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {faBars} from '@fortawesome/free-solid-svg-icons';
import {CajaDiariaService} from '../../services';
import {ResponseData} from '../../../global/interfaces';
import {CajaDiaria, CerrarCaja} from '../../interfaces';

@Component({
  selector: 'app-cerrar-caja',
  templateUrl: './cerrar-caja.component.html'
})
export class CerrarCajaComponent implements OnInit {
  faBars = faBars;
  @Input()
  cajaDiariaId: string = '';
  @Output()
  responseData = new EventEmitter<ResponseData<CajaDiaria>>();
  cerrarCajaForm: FormGroup = this.fb.group({
    totalContabilizado: [0],
    totalCierre: [0],
  });
  cerrarCajaData: CerrarCaja = {
    totalContabilizado: 0,
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
