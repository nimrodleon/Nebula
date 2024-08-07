import {Component, OnInit} from "@angular/core";
import {faEdit, faFilter, faSave, faSearch, faTrashAlt, faUserPlus} from "@fortawesome/free-solid-svg-icons";
import {FormBuilder, FormControl, ReactiveFormsModule} from "@angular/forms";
import {CollaboratorService} from "../../services/collaborator.service";
import {Collaborator, CollaboratorResponse, InviteCollaborator} from "../../interfaces";
import {ActivatedRoute} from "@angular/router";
import _ from "lodash";
import {deleteConfirm, toastError, toastSuccess} from "app/common/interfaces";
import {NgForOf} from "@angular/common";
import {
  CompanyDetailContainerComponent
} from "app/common/containers/company-detail-container/company-detail-container.component";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";
import {LoaderComponent} from "app/common/loader/loader.component";
import {CollaboratorAddModalComponent} from "../../components/collaborator-add-modal/collaborator-add-modal.component";

declare const bootstrap: any;

@Component({
  selector: "app-collaborator-list",
  standalone: true,
  imports: [
    NgForOf,
    CompanyDetailContainerComponent,
    FaIconComponent,
    LoaderComponent,
    CollaboratorAddModalComponent,
    ReactiveFormsModule
  ],
  templateUrl: "./collaborator-list.component.html"
})
export class CollaboratorListComponent implements OnInit {
  faFilter = faFilter;
  faSearch = faSearch;
  faUserPlus = faUserPlus;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  faSave = faSave;
  companyId: string = "";
  collaboratorAddModal: any;
  cambiarRolModal: any;
  userRole: FormControl = this.fb.control("");
  currentCollaborator: CollaboratorResponse = new CollaboratorResponse();
  collaborators: CollaboratorResponse[] = [];
  loading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private collaboratorService: CollaboratorService) {
  }

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
      this.collaboratorService.index(this.companyId)
        .subscribe(result => this.collaborators = result);
    });
    this.collaboratorAddModal = new bootstrap.Modal("#collaborator-add-modal");
    this.cambiarRolModal = new bootstrap.Modal("#cambiarRolModal");
  }

  public showInvitarModal(): void {
    this.collaboratorAddModal.show();
  }

  public invitarColaborador(data: InviteCollaborator): void {
    this.loading = true;
    this.collaboratorService.invite(data)
      .subscribe(result => {
        this.loading = false;
        if (!result.ok) {
          toastError("La invitación no pudo ser enviada!");
        } else {
          toastSuccess(result.msg);
          this.collaboratorAddModal.hide();
        }
      });
  }

  public cambiarRolShow(item: CollaboratorResponse): void {
    this.currentCollaborator = item;
    this.userRole.setValue(item.userRole);
    this.cambiarRolModal.show();
  }

  public guardarNuevoRol(): void {
    const collaborator: Collaborator = new Collaborator();
    collaborator.id = this.currentCollaborator.collaboratorId;
    collaborator.companyId = this.currentCollaborator.companyId;
    collaborator.userId = this.currentCollaborator.userId;
    collaborator.userRole = this.userRole.value;
    this.collaboratorService.update(collaborator.id, collaborator)
      .subscribe(result => {
        this.collaborators = _.map(this.collaborators, item => {
          if (item.collaboratorId === result.id) item.userRole = result.userRole;
          return item;
        });
        this.cambiarRolModal.hide();
      });
  }

  public borrarColaborador(id: string): void {
    deleteConfirm().then(result => {
      if (result.isConfirmed) {
        this.collaboratorService.delete(id).subscribe(result => {
          this.collaborators = _.filter(this.collaborators, item => item.collaboratorId !== result.id);
        });
      }
    });
  }

}
