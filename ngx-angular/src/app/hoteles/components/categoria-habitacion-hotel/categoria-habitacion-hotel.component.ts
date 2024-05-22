import { NgClass } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { FormType, toastError } from 'app/common/interfaces';
import { CategoriaHabitacion, CategoriaHabitacionDataModal } from 'app/hoteles/interfaces';
import { CategoriaHabitacionService } from 'app/hoteles/services';

@Component({
  selector: 'app-categoria-habitacion-hotel',
  standalone: true,
  imports: [
    NgClass,
    ReactiveFormsModule
  ],
  templateUrl: './categoria-habitacion-hotel.component.html'
})
export class CategoriaHabitacionHotelComponent implements OnInit {
  private fb: FormBuilder = inject(FormBuilder);
  private categoriaHabitacionService: CategoriaHabitacionService = inject(CategoriaHabitacionService);
  @Input()
  data: CategoriaHabitacionDataModal = new CategoriaHabitacionDataModal();
  @Output()
  responseData = new EventEmitter<CategoriaHabitacionDataModal>();

  categoriaHabitacionForm = this.fb.group({
    nombre: ["", Validators.required],
    estado: ["ACTIVO"]
  });

  ngOnInit(): void {
    const myModal: any = document.querySelector("#categoriaHabitacionModal");
    myModal.addEventListener("shown.bs.modal", () => {
      this.categoriaHabitacionForm.reset(this.data.categoriaHabitacion);
    });
    myModal.addEventListener("hide.bs.modal", () => {
      this.categoriaHabitacionForm.reset(new CategoriaHabitacion());
    });
  }

  public inputIsInvalid(field: string) {
    const control = this.categoriaHabitacionForm.get(field);
    return control && control.errors && control.touched;
  }

  public saveChanges(): void {
    if (this.categoriaHabitacionForm.invalid) {
      this.categoriaHabitacionForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    const { nombre, estado } = this.categoriaHabitacionForm.value;
    if (this.data.type === FormType.ADD) {
      this.categoriaHabitacionService.create({
        ...this.data.categoriaHabitacion,
        nombre: nombre || "", estado: estado || ""
      }).subscribe(result => {
        this.data.categoriaHabitacion = result;
        this.responseData.emit(this.data);
      });
    }
    if (this.data.type === FormType.EDIT) {
      const { id } = this.data.categoriaHabitacion;
      this.categoriaHabitacionService.update(id, {
        ...this.data.categoriaHabitacion,
        nombre: nombre || "", estado: estado || "", id
      }).subscribe(result => {
        this.data.categoriaHabitacion = result;
        this.responseData.emit(this.data);
      });
    }
  }
}
