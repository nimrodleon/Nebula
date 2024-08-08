import {Component, EventEmitter, Input, OnInit, Output} from "@angular/core";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {Category, CategoryDataModal} from "../../interfaces";
import {NgClass} from "@angular/common";
import {toastError} from "../../../common/interfaces";

@Component({
  selector: "app-category-modal",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgClass
  ],
  templateUrl: "./category-modal.component.html"
})
export class CategoryModalComponent implements OnInit {
  @Input()
  categoryDataModal = new CategoryDataModal();
  @Output()
  responseData = new EventEmitter<CategoryDataModal>();
  categoryForm: FormGroup = this.fb.group({
    id: [null],
    name: ["", [Validators.required]]
  });

  constructor(private fb: FormBuilder) {
  }

  ngOnInit(): void {
    const myModal: any = document.querySelector("#category-modal");
    myModal.addEventListener("shown.bs.modal", () => {
      this.categoryForm.reset(this.categoryDataModal.category);
    });
    myModal.addEventListener("hide.bs.modal", () => {
      this.categoryForm.reset(new Category());
    });
  }

  // Verificar campo invalido.
  public inputIsInvalid(field: string) {
    return this.categoryForm.controls[field].errors && this.categoryForm.controls[field].touched;
  }

  // guardar cambios.
  public saveChanges(): void {
    if (this.categoryForm.invalid) {
      this.categoryForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    // Guardar datos, sólo si es válido el formulario.
    const {id} = this.categoryDataModal.category;
    this.categoryDataModal.category = {...this.categoryForm.value, id};
    this.responseData.emit(this.categoryDataModal);
  }

}
