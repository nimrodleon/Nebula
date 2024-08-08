import {Component, Injector, Input, OnInit} from "@angular/core";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {ActivatedRoute, Router} from "@angular/router";
import {
  faCalculator,
  faCircleCheck,
  faCircleLeft,
  faEdit,
  faMessage,
  faPlus,
  faRightFromBracket,
  faSave,
  faTrashAlt
} from "@fortawesome/free-solid-svg-icons";
import {Material, MaterialDataModal, MaterialDetail} from "../../interfaces";
import {
  confirmTask, deleteConfirm, select2Contactos, initializeSelect2Injector, accessDenied, toastError, toastSuccess
} from "app/common/interfaces";
import {MaterialDetailService, MaterialService} from "../../services";
import {UserDataService} from "app/common/user-data.service";
import _ from "lodash";
import moment from "moment";
import Swal from "sweetalert2";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {DatePipe, NgClass, NgForOf, NgIf} from "@angular/common";
import {MaterialModalComponent} from "../material-modal/material-modal.component";

declare const bootstrap: any;

@Component({
  selector: "app-materiales-form",
  standalone: true,
  imports: [
    FaIconComponent,
    ReactiveFormsModule,
    NgClass,
    DatePipe,
    NgForOf,
    NgIf,
    MaterialModalComponent
  ],
  templateUrl: "./materiales-form.component.html"
})
export class MaterialesFormComponent implements OnInit {
  @Input()
  type: "ADD" | "EDIT" = "ADD";
  faSave = faSave;
  faCircleLeft = faCircleLeft;
  faMessage = faMessage;
  faPlus = faPlus;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  faCircleCheck = faCircleCheck;
  faRightFromBracket = faRightFromBracket;
  faCalculator = faCalculator;
  companyId: string = "";
  materialModal: any;
  dataModal = new MaterialDataModal();
  material: Material = new Material();
  materialForm: FormGroup = this.fb.group({
    contactId: [null, [Validators.required]],
    employeeId: [null, [Validators.required]],
    remark: [null, [Validators.required]],
  });
  materialDetails: Array<MaterialDetail> = new Array<MaterialDetail>();

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private injector: Injector,
    private activatedRoute: ActivatedRoute,
    private userDataService: UserDataService,
    private materialService: MaterialService,
    private materialDetailService: MaterialDetailService) {
    initializeSelect2Injector(injector);
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
    });
    this.materialModal = new bootstrap.Modal("#material-modal");
    const contacto = select2Contactos("#contactId")
      .on("select2:select", (e: any) => {
        const data = e.params.data;
        this.materialForm.controls["contactId"].setValue(data.id);
        this.material.contactId = data.id;
        this.material.contactName = data.name;
      });
    const trabajador = select2Contactos("#employeeId")
      .on("select2:select", (e: any) => {
        const data = e.params.data;
        this.materialForm.controls["employeeId"].setValue(data.id);
        this.material.employeeId = data.id;
        this.material.employeeName = data.name;
      });
    if (this.type === "EDIT") {
      const id: string = this.activatedRoute.snapshot.params["id"];
      this.materialService.show(id).subscribe(result => {
        this.material = result;
        this.materialForm.reset({
          contactId: result.contactId,
          employeeId: result.employeeId,
          remark: result.remark
        });
        // reset input select2.
        const {contactId, contactName} = result;
        const contactOption = new Option(contactName, contactId, true, true);
        contacto.append(contactOption).trigger("change");
        const {employeeId, employeeName} = result;
        const employeeOption = new Option(employeeName, employeeId, true, true);
        trabajador.append(employeeOption).trigger("change");
      });
      this.materialDetailService.index(id)
        .subscribe(result => this.materialDetails = result);
    }
  }

  public inputIsInvalid(field: string) {
    return this.materialForm.controls[field].errors
      && this.materialForm.controls[field].touched;
  }

  public saveChange(): void {
    this.material = {...this.material, ...this.materialForm.value};
    if (this.materialForm.invalid) {
      this.materialForm.markAllAsTouched();
      toastError("Ingrese la información en todos los campos requeridos!");
      return;
    }
    if (this.type === "ADD") {
      this.material.user = "this.authUser.user.userName;"; // todo: refactoring.
      this.material.createdAt = moment().format("YYYY-MM-DD");
      this.materialService.create(this.material)
        .subscribe(result => {
          this.router.navigate([
            "/", this.companyId, "inventories", "materiales", "edit", result.id
          ]).then(() => {
            toastSuccess("Nueva salida de materiales registrado!");
          });
        });
    }
    if (this.type === "EDIT") {
      this.materialService.update(this.material.id, this.material)
        .subscribe(result => {
          this.material = result;
          this.materialForm.reset({
            contactId: result.contactId,
            employeeId: result.employeeId,
            remark: result.remark
          });
          toastSuccess("Los cambios han sido guardados!");
        });
    }
  }

  public saveChangeDetail(data: MaterialDataModal): void {
    if (this.type === "EDIT") {
      data.materialDetail.materialId = this.material.id;
      if (data.type === "ADD") {
        this.materialDetailService.create(data.materialDetail)
          .subscribe(result => {
            this.materialDetails = _.concat(this.materialDetails, result);
            this.materialModal.hide();
            toastSuccess("Nuevo producto agregado a la tabla!");
          });
      }
      if (data.type === "EDIT") {
        this.materialDetailService.update(data.materialDetail.id, data.materialDetail)
          .subscribe(result => {
            this.materialDetails = _.map(this.materialDetails, (o: MaterialDetail) => {
              if (o.id === result.id) o = result;
              return o;
            });
            this.materialModal.hide();
            toastSuccess("El producto de la tabla ha sido actualizado!");
          });
      }
    }
  }

  public cambiarCantidadRetorno(item: MaterialDetail): void {
    Swal.fire({
      title: "CANTIDAD DE RETORNO",
      input: "number",
      inputLabel: "INGRESE CANTIDAD DEL RETORNO",
      inputAttributes: {"step": "1"},
      showCancelButton: true,
      confirmButtonText: "Sí, Aceptar",
      cancelButtonText: "Cancelar",
      inputValidator(value: string): any {
        if (!value || isNaN(Number(value))) {
          return "Debe ingresar un número válido.";
        }
      }
    }).then(result => {
      if (result.isConfirmed) {
        const cantRetorno = Number(result.value);
        item.cantRetorno = cantRetorno;
        item.cantUsado = item.cantSalida - cantRetorno;
        this.materialDetailService.update(item.id, item)
          .subscribe(() => {
            toastSuccess("Las cantidades del producto han sido actualizados!");
          });
      }
    });
  }

  public deleteDetail(id: string): void {
    deleteConfirm().then(result => {
      if (result.isConfirmed) {
        if (this.material.status === "BORRADOR") {
          this.materialDetailService.delete(id)
            .subscribe(result => {
              this.materialDetails = _.filter(this.materialDetails,
                (o: MaterialDetail) => o.id !== result.id);
            });
        }
        if (this.material.status === "VALIDADO") {
          if (!this.userDataService.canDelete()) {
            accessDenied().then(() => console.log("accessDenied"));
          } else {
            this.materialDetailService.delete(id)
              .subscribe(result => {
                this.materialDetails = _.filter(this.materialDetails,
                  (o: MaterialDetail) => o.id !== result.id);
              });
          }
        }
      }
    });
  }

  public showMaterialModal(): void {
    this.dataModal.type = "ADD";
    this.dataModal.title = "Agregar Material";
    this.dataModal.materialDetail = new MaterialDetail();
    this.materialModal.show();
  }

  public editMaterialModal(data: MaterialDetail): void {
    this.dataModal.type = "EDIT";
    this.dataModal.title = "Editar Material";
    this.dataModal.materialDetail = data;
    this.materialModal.show();
  }

  public back(): void {
    this.router.navigate([
      "/", this.companyId, "inventories", "materiales"]).then(() => console.log("back"));
  }

  public validar(): void {
    confirmTask().then(result => {
      if (result.isConfirmed) {
        this.materialService.validate(this.material.id)
          .subscribe(result => {
            this.material = result.material;
            this.materialDetails = result.materialDetails;
            toastSuccess("La validación ha sido exitosa!");
          });
      }
    });
  }

  public validButton(): boolean {
    return this.type === "EDIT" && this.material.status === "BORRADOR";
  }

  public invalidEditItem(): boolean {
    return this.type === "ADD" || this.material.status === "VALIDADO";
  }

}
