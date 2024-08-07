import {Component, OnInit} from "@angular/core";
import {ActivatedRoute} from "@angular/router";
import {faFileUpload, faLock} from "@fortawesome/free-solid-svg-icons";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {CompanyService} from "app/account/services";
import {Company} from "app/account/interfaces";
import {catchError} from "rxjs/operators";
import {
  CompanyDetailContainerComponent
} from "app/common/containers/company-detail-container/company-detail-container.component";
import {DatePipe, NgClass, NgIf} from "@angular/common";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {LoaderComponent} from "app/common/loader/loader.component";
import {toastError, toastSuccess} from "../../../../common/interfaces";

@Component({
  selector: "app-certificado",
  standalone: true,
  imports: [
    CompanyDetailContainerComponent,
    NgIf,
    NgClass,
    ReactiveFormsModule,
    FaIconComponent,
    DatePipe,
    LoaderComponent
  ],
  templateUrl: "./certificado.component.html"
})
export class CertificadoComponent implements OnInit {
  faFileUpload = faFileUpload;
  faLock = faLock;
  companyId: string = "";
  certificadoForm: FormGroup;
  certificateFile: any;
  fileExtension: any = "";
  company: Company = new Company();
  loading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private companyService: CompanyService,) {
    this.certificadoForm = this.fb.group({
      certificate: ["", Validators.required],
      password: ["", Validators.required]
    });
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
      this.companyService.getCompanyById(this.companyId)
        .subscribe(result => this.company = result);
    });
  }

  // Verificar campo invalido.
  public inputIsInvalid(field: string) {
    return this.certificadoForm.controls[field].errors
      && this.certificadoForm.controls[field].touched;
  }

  public selectedCertificate(event: any): void {
    this.certificateFile = event.target.files[0];
    // Obtener la extensión del archivo
    const fileName: string = this.certificateFile.name;
    this.fileExtension = fileName.split(".").pop();
  }

  public subirCertificado(): void {
    if (this.certificadoForm.invalid) {
      this.certificadoForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    this.loading = true;
    const formData = new FormData();
    formData.append("certificate", this.certificateFile);
    formData.append("password", this.certificadoForm.get("password")?.value);
    formData.append("extension", this.fileExtension);

    this.companyService.subirCertificado(this.companyId, formData)
      .pipe(catchError(err => {
        this.loading = false;
        toastError(err.error.msg);
        throw err;
      })).subscribe(result => {
      this.company = result;
      toastSuccess("El certificado se ha subido correctamente!");
      this.loading = false;
    });
  }

  public sincronizarDatos(): void {
    this.loading = true;
    this.companyService.sincronizarDatos(this.companyId)
      .pipe(catchError(err => {
        this.loading = false;
        throw err;
      })).subscribe(result => {
      this.company = result;
      toastSuccess("Sincronización Completada!");
      this.loading = false;
    });
  }

  public quitarCertificado(): void {
    this.loading = true;
    this.companyService.quitarCertificado(this.companyId)
      .subscribe(result => {
        this.company = result;
        toastSuccess("El certificado se ha quitado!");
        this.loading = false;
      });
  }

  public cambiarSunatEndpoint(): void {
    this.loading = true;
    this.companyService.cambiarSunatEndpoint(this.companyId)
      .pipe(catchError(err => {
        this.loading = false;
        toastError(err.error.msg);
        throw err;
      })).subscribe(result => {
      this.company = result;
      toastSuccess("Se ha cambiado el EndPoint correctamente!");
      this.loading = false;
    });
  }

}
