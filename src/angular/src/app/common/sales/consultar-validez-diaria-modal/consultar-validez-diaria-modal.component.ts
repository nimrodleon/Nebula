import {Component, EventEmitter, Output} from "@angular/core";
import {faCircleDown} from "@fortawesome/free-solid-svg-icons";
import {FormBuilder, FormControl, ReactiveFormsModule} from "@angular/forms";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";

@Component({
  selector: "app-consultar-validez-diaria-modal",
  standalone: true,
  imports: [
    FaIconComponent,
    ReactiveFormsModule
  ],
  templateUrl: "./consultar-validez-diaria-modal.component.html"
})
export class ConsultarValidezDiariaModalComponent {
  faCircleDown = faCircleDown;
  @Output()
  responseData = new EventEmitter<string>();
  fecha: FormControl = this.fb.control("");

  constructor(private fb: FormBuilder) {
  }

  public saveChanges(): void {
    return this.responseData.emit(this.fecha.value);
  }

}
