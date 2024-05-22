import { NgClass } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { FormType, toastError } from 'app/common/interfaces';
import { PisoHotel, PisoHotelDataModal } from 'app/hoteles/interfaces';
import { PisoHotelService } from 'app/hoteles/services';

@Component({
  selector: 'app-piso-hotel-modal',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: './piso-hotel-modal.component.html',
})
export class PisoHotelModalComponent implements OnInit {
  private fb: FormBuilder = inject(FormBuilder);
  private pisoHotelService: PisoHotelService = inject(PisoHotelService);
  @Input()
  data: PisoHotelDataModal = new PisoHotelDataModal();
  @Output()
  responseData = new EventEmitter<PisoHotelDataModal>();

  pisoHotelForm = this.fb.group({
    nombre: ["", Validators.required],
    estado: ["ACTIVO"]
  });

  ngOnInit(): void {
    const myModal: any = document.querySelector("#pisoHotelModal");
    myModal.addEventListener("shown.bs.modal", () => {
      this.pisoHotelForm.reset(this.data.pisoHotel);
    });
    myModal.addEventListener("hide.bs.modal", () => {
      this.pisoHotelForm.reset(new PisoHotel());
    });
  }

  public inputIsInvalid(field: string) {
    const control = this.pisoHotelForm.get(field);
    return control && control.errors && control.touched;
  }

  public saveChanges(): void {
    if (this.pisoHotelForm.invalid) {
      this.pisoHotelForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    const { nombre, estado } = this.pisoHotelForm.value;
    if (this.data.type === FormType.ADD) {
      this.pisoHotelService.create({
        ...this.data.pisoHotel,
        nombre: nombre || "", estado: estado || ""
      }).subscribe(result => {
        this.data.pisoHotel = result;
        this.responseData.emit(this.data);
      });
    }
    if (this.data.type === FormType.EDIT) {
      const {id} = this.data.pisoHotel;
      this.pisoHotelService.update(id, {
        ...this.data.pisoHotel,
        nombre: nombre || "", estado: estado || "", id
      }).subscribe(result => {
        this.data.pisoHotel = result;
        this.responseData.emit(this.data);
      });
    }
  }

}
