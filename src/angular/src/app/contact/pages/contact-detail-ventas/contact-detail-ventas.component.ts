import {Component, OnInit} from "@angular/core";
import {ActivatedRoute, Router, RouterLink} from "@angular/router";
import {FormBuilder, FormGroup, ReactiveFormsModule} from "@angular/forms";
import {faPlus, faSearch} from "@fortawesome/free-solid-svg-icons";
import {ContactDetailService} from "../../services";
import moment from "moment/moment";
import {
  ContactDetailContainerComponent
} from "../../components/contact-detail-container/contact-detail-container.component";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {GetInvoiceTypePipe} from "../../../common/pipes/get-invoice-type.pipe";
import {CurrencyPipe, DatePipe, NgForOf} from "@angular/common";

@Component({
  selector: "app-contact-detail-ventas",
  standalone: true,
  imports: [
    ContactDetailContainerComponent,
    ReactiveFormsModule,
    FaIconComponent,
    GetInvoiceTypePipe,
    DatePipe,
    NgForOf,
    RouterLink,
    CurrencyPipe
  ],
  templateUrl: "./contact-detail-ventas.component.html"
})
export class ContactDetailVentasComponent implements OnInit {
  companyId: string = "";
  faSearch = faSearch;
  faPlus = faPlus;
  queryForm: FormGroup = this.fb.group({
    year: [moment().format("YYYY")],
    month: [moment().format("MM")],
  });

  constructor(
    private router: Router,
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

  public get registroDeVentas() {
    return this.contactDetailService.registroDeVentas;
  }

  public cargarRegistroDeVentas(): void {
    const year = this.queryForm.get("year")?.value;
    const month = this.queryForm.get("month")?.value;
    this.contactDetailService.getRegistroDeVentas(this.contact.id, month, year);
  }

  public nuevoComprobante(): void {
    this.router.navigate([
      "/", this.companyId, "sales", "form"
    ], {
      queryParams: {
        origin: 1,
        cid: this.contact.id,
      }
    }).then(() => console.log("nuevoComprobante"));
  }
}
