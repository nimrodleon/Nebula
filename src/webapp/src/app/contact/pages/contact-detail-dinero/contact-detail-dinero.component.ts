import {Component} from "@angular/core";
import {FormBuilder, FormGroup, ReactiveFormsModule} from "@angular/forms";
import {faSearch} from "@fortawesome/free-solid-svg-icons";
import moment from "moment/moment";
import {ContactDetailService} from "../../services";
import {
  ContactDetailContainerComponent
} from "../../components/contact-detail-container/contact-detail-container.component";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {DatePipe, NgForOf, NgIf} from "@angular/common";

@Component({
  selector: "app-contact-detail-dinero",
  standalone: true,
  imports: [
    ContactDetailContainerComponent,
    ReactiveFormsModule,
    FaIconComponent,
    NgForOf,
    DatePipe,
    NgIf
  ],
  templateUrl: "./contact-detail-dinero.component.html"
})
export class ContactDetailDineroComponent {
  faSearch = faSearch;
  queryForm: FormGroup = this.fb.group({
    year: [moment().format("YYYY")],
    month: [moment().format("MM")],
  });

  constructor(
    private fb: FormBuilder,
    private contactDetailService: ContactDetailService) {
  }

  public get contact() {
    return this.contactDetailService.contact;
  }

  public get entradaSalida() {
    return this.contactDetailService.entradaSalida;
  }

  public cargarEntradaSalidaDeDinero(): void {
    const year = this.queryForm.get("year")?.value;
    const month = this.queryForm.get("month")?.value;
    this.contactDetailService.getEntradaSalidaDeDinero(month, year);
  }

}
