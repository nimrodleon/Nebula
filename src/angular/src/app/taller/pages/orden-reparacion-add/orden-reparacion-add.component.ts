import {Component} from '@angular/core';
import {FormType} from 'app/common/interfaces';
import {OrdenReparacionFormComponent} from "../../components/orden-reparacion-form/orden-reparacion-form.component";
import {TallerContainerComponent} from "app/common/containers/taller-container/taller-container.component";

@Component({
  selector: "app-orden-reparacion-add",
  standalone: true,
  imports: [
    OrdenReparacionFormComponent,
    TallerContainerComponent
  ],
  templateUrl: "./orden-reparacion-add.component.html"
})
export class OrdenReparacionAddComponent {
  protected readonly EnumTypeForm = FormType;
}
