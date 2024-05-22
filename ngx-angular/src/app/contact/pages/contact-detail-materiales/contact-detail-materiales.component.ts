import {Component, OnInit} from "@angular/core";
import {FormBuilder, FormGroup, ReactiveFormsModule} from "@angular/forms";
import moment from "moment/moment";
import {faEdit, faSearch, faTrashAlt} from "@fortawesome/free-solid-svg-icons";
import {ContactDetailService} from "../../services";
import {ActivatedRoute, RouterLink} from "@angular/router";
import {
  ContactDetailContainerComponent
} from "../../components/contact-detail-container/contact-detail-container.component";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {DatePipe, NgForOf} from "@angular/common";

@Component({
  selector: "app-contact-detail-materiales",
  standalone: true,
  imports: [
    ContactDetailContainerComponent,
    ReactiveFormsModule,
    FaIconComponent,
    DatePipe,
    NgForOf,
    RouterLink
  ],
  templateUrl: "./contact-detail-materiales.component.html"
})
export class ContactDetailMaterialesComponent implements OnInit {
  companyId: string = "";
  faSearch = faSearch;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  queryForm: FormGroup = this.fb.group({
    year: [moment().format("YYYY")],
    month: [moment().format("MM")],
  });

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private contactDetailService: ContactDetailService) {
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
    });
  }

  public get contact() {
    return this.contactDetailService.contact;
  }

  public get materials() {
    return this.contactDetailService.materials;
  }

  public cargarMateriales(): void {
    const year = this.queryForm.get("year")?.value;
    const month = this.queryForm.get("month")?.value;
    this.contactDetailService.getMaterials(month, year);
  }

  public delete(id: string): void {
    this.contactDetailService.deleteMaterial(id);
  }

}
