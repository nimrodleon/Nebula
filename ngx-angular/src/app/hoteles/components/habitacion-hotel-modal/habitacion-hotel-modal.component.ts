import { NgClass, NgFor } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { FormType, toastError } from 'app/common/interfaces';
import { CategoriaHabitacion, HabitacionHotelDataModal, PisoHotel } from 'app/hoteles/interfaces';
import { CategoriaHabitacionService, HabitacionHotelService, PisoHotelService } from 'app/hoteles/services';

@Component({
  selector: 'app-habitacion-hotel-modal',
  standalone: true,
  imports: [
    NgFor,
    NgClass,
    ReactiveFormsModule
  ],
  templateUrl: './habitacion-hotel-modal.component.html',
})
export class HabitacionHotelModalComponent implements OnInit {
  private fb: FormBuilder = inject(FormBuilder);
  private habitacionHotelService: HabitacionHotelService = inject(HabitacionHotelService);
  private pisoHotelService: PisoHotelService = inject(PisoHotelService);
  private categoriaHabitacionService: CategoriaHabitacionService = inject(CategoriaHabitacionService);
  pisosHotel: Array<PisoHotel> = new Array<PisoHotel>();
  categoriasHabitacion: Array<CategoriaHabitacion> = new Array<CategoriaHabitacion>();
  hoursRange: number[] = Array.from({ length: 24 }, (_, index) => index + 1);
  @Input()
  data: HabitacionHotelDataModal = new HabitacionHotelDataModal();
  @Output()
  responseData = new EventEmitter<HabitacionHotelDataModal>();

  habitacionHotelForm = this.fb.group({
    nombre: ["", Validators.required],
    pisoHotelId: ["", Validators.required],
    categoriaHabitacionId: ["", Validators.required],
    precio: [0, Validators.required],
    tarifaHoras: [0, Validators.required],
    estado: ["DISPONIBLE", Validators.required],
    remark: [""]
  });

  ngOnInit(): void {
    this.cargarPisosHotel();
    this.cargarCategoriasHabitacion();
    const myModal: any = document.querySelector("#habitacionHotelModal");
    myModal.addEventListener("shown.bs.modal", () => {
      this.habitacionHotelForm.reset(this.data.habitacionHotel);
    });
    myModal.addEventListener("hide.bs.modal", () => {
      this.habitacionHotelForm.reset(new PisoHotel());
    });
  }

  public inputIsInvalid(field: string) {
    const control = this.habitacionHotelForm.get(field);
    return control && control.errors && control.touched;
  }

  private cargarPisosHotel(): void {
    this.pisoHotelService.index().subscribe(result => this.pisosHotel = result);
  }

  private cargarCategoriasHabitacion(): void {
    this.categoriaHabitacionService.index().subscribe(result => this.categoriasHabitacion = result);
  }

  public saveChanges(): void {
    if (this.habitacionHotelForm.invalid) {
      this.habitacionHotelForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    const {
      nombre,
      pisoHotelId,
      categoriaHabitacionId,
      precio,
      tarifaHoras,
      estado,
      remark
    } = this.habitacionHotelForm.value;
    if (this.data.type === FormType.ADD) {
      this.habitacionHotelService.create({
        ...this.data.habitacionHotel,
        nombre: nombre || "",
        pisoHotelId: pisoHotelId || "",
        categoriaHabitacionId: categoriaHabitacionId || "",
        precio: precio || 0,
        tarifaHoras: tarifaHoras || 24,
        estado: estado || "",
        remark: remark || ""
      }).subscribe(result => {
        this.data.habitacionHotel = result;
        this.responseData.emit(this.data);
      });
    }
    if (this.data.type === FormType.EDIT) {
      const { id } = this.data.habitacionHotel;
      this.habitacionHotelService.update(id, {
        ...this.data.habitacionHotel,
        nombre: nombre || "",
        pisoHotelId: pisoHotelId || "",
        categoriaHabitacionId: categoriaHabitacionId || "",
        precio: precio || 0,
        tarifaHoras: tarifaHoras || 24,
        estado: estado || "",
        remark: remark || "",
        id
      }).subscribe(result => {
        this.data.habitacionHotel = result;
        this.responseData.emit(this.data);
      });
    }
  }


}
