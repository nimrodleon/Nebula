import {Component} from "@angular/core";
import {FormType} from "app/common/interfaces";
import {OrdenReparacionFormComponent} from "../../components/orden-reparacion-form/orden-reparacion-form.component";
import {TallerContainerComponent} from "../../../common/containers/taller-container/taller-container.component";

@Component({
  selector: "app-orden-reparacion-edit",
  standalone: true,
  imports: [
    OrdenReparacionFormComponent,
    TallerContainerComponent
  ],
  templateUrl: "./orden-reparacion-edit.component.html"
})
export class OrdenReparacionEditComponent {
  protected readonly EnumTypeForm = FormType;
}
