import { Component, Input, OnInit } from "@angular/core";
import { faCircleLeft, faClipboardList, faCog, faLocationCrosshairs, faLock, faSave, } from "@fortawesome/free-solid-svg-icons";
import { ActivatedRoute, Router, RouterLink } from "@angular/router";
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { UserDataService } from "app/common/user-data.service";
import { FormType, select2Contactos, toastError, toastSuccess } from "app/common/interfaces";
import { ContactService } from "app/contact/services";
import { CompanyService } from "../../services";
import { Company } from "../../interfaces";
import { NgClass } from "@angular/common";
import { FaIconComponent } from "@fortawesome/angular-fontawesome";

@Component({
  selector: "app-company-form",
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgClass,
    FaIconComponent,
    RouterLink
  ],
  templateUrl: "./company-form.component.html"
})
export class CompanyFormComponent implements OnInit {
  @Input()
  formType: string = FormType.ADD;
  faClipboardList = faClipboardList;
  faCircleLeft = faCircleLeft;
  faSave = faSave;
  faLocationCrosshairs = faLocationCrosshairs;
  faLock = faLock;
  faCog = faCog;

  company: Company = new Company();
  companyForm: FormGroup = this.fb.group({
    id: [undefined],
    ruc: ["", Validators.required],
    rznSocial: ["", Validators.required],
    address: ["", Validators.required],
    phoneNumber: ["-"],
    anchoTicket: ["410"],
    codLocalEmisor: ["0000"],
    tipMoneda: ["PEN"],
    porcentajeIgv: [18],
    valorImpuestoBolsa: [0.5],
    contactId: [""],
    diasPlazo: [30],
    ubigueo: [""],
    departamento: [""],
    provincia: [""],
    distrito: [""],
    urbanizacion: [""],
    claveSol: this.fb.group({
      user: [""],
      password: [""]
    }),
    modInventarios: [false],
    modComprobantes: [false],
    modCuentaPorCobrar: [false],
    modReparaciones: [false],
    modCajasDiaria: [false],
    emitirModoRus: [false],
  });

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private userDataService: UserDataService,
    private activatedRoute: ActivatedRoute,
    private contactService: ContactService,
    private companyService: CompanyService) {
  }

  ngOnInit(): void {
    if (this.formType === FormType.EDIT) {
      const id: string = this.activatedRoute.snapshot.params["id"];
      // configurar select2 para el campo contactId.
      const contacts = select2Contactos("#contactId", undefined, id)
        .on("select2:select", ({ params }: any) => {
          this.companyForm.controls["contactId"].setValue(params.data.id);
        });
      this.companyService.getCompanyById(id)
        .subscribe(result => {
          this.company = result;
          this.companyForm.reset({ ...result });
          // cargar cliente por defecto.
          if (result.contactId) {
            this.contactService.show(result.contactId, id)
              .subscribe(result => {
                const newOption = new Option(`${result.document} - ${result.name}`, <any>result.id, true, true);
                contacts.append(newOption).trigger("change");
              });
          }
        });
    }
  }

  // Verificar campo invalido.
  public inputIsInvalid(field: string) {
    return this.companyForm.controls[field].errors
      && this.companyForm.controls[field].touched;
  }

  public saveChanges(): void {
    if (this.companyForm.invalid) {
      this.companyForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    this.company = { ...this.company, ...this.companyForm.value };
    if (this.formType === FormType.ADD) {
      this.company.id = undefined;
      this.companyService.createCompany(this.company)
        .subscribe(result => {
          this.router.navigate(["/account/companies"])
            .then(() => {
              this.userDataService.cargarData();
              toastSuccess("La empresa ha sido creada!");
            });
        });
    }
    if (this.formType === FormType.EDIT) {
      this.companyService.updateCompany(this.company.id, this.company)
        .subscribe(result => {
          this.userDataService.cargarData();
          this.company = result;
          this.companyForm.reset({ ...result });
          toastSuccess("La empresa ha sido actualizada!");
        });
    }
  }

}
