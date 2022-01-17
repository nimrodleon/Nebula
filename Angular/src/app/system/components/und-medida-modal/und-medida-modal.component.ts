import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {faBars} from '@fortawesome/free-solid-svg-icons';
import {ResponseData} from 'src/app/global/interfaces';
import {UndMedidaService} from '../../services';
import {UndMedida} from '../../interfaces';

@Component({
  selector: 'app-und-medida-modal',
  templateUrl: './und-medida-modal.component.html',
  styleUrls: ['./und-medida-modal.component.scss']
})
export class UndMedidaModalComponent implements OnInit {
  faBars = faBars;
  @Input()
  title: string = '';
  @Input()
  undMedida: UndMedida = new UndMedida();
  @Output()
  responseData = new EventEmitter<ResponseData<UndMedida>>();
  undMedidaForm: FormGroup = this.fb.group({
    id: [null],
    name: ['', [Validators.required]],
    sunatCode: ['', [Validators.required]]
  });

  constructor(
    private fb: FormBuilder,
    private undMedidaService: UndMedidaService) {
  }

  ngOnInit(): void {
    const myModal: any = document.querySelector('#und-medida-modal');
    myModal.addEventListener('shown.bs.modal', () => {
      if (this.undMedida !== null) {
        this.undMedidaForm.reset(this.undMedida);
      }
    });
    myModal.addEventListener('hide.bs.modal', () => {
      this.undMedidaForm.addControl('id', new FormControl(null));
      this.undMedidaForm.reset();
    });
  }

  // Verificar campo invalido.
  public inputIsInvalid(field: string) {
    return this.undMedidaForm.controls[field].errors && this.undMedidaForm.controls[field].touched;
  }

  // guardar cambios.
  public saveChanges(): void {
    if (this.undMedidaForm.invalid) {
      this.undMedidaForm.markAllAsTouched();
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    if (this.undMedidaForm.get('id')?.value === null) {
      this.undMedidaForm.removeControl('id');
      this.undMedidaService.create(this.undMedidaForm.value)
        .subscribe(result => this.responseData.emit(result));
    } else {
      const id = this.undMedidaForm.get('id')?.value;
      this.undMedidaService.update(id, this.undMedidaForm.value)
        .subscribe(result => this.responseData.emit(result));
    }
  }

}
