import {Component, OnInit} from "@angular/core";
import {faCircleLeft, faSadTear, faThumbsUp} from "@fortawesome/free-solid-svg-icons";
import {CollaboratorService} from "app/account/company/services/collaborator.service";
import {ActivatedRoute, RouterLink} from "@angular/router";
import {catchError} from "rxjs/operators";
import {NgIf} from "@angular/common";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";

@Component({
  selector: "app-validate-employee",
  templateUrl: "./validate-employee.component.html",
  standalone: true,
  imports: [
    NgIf,
    FaIconComponent,
    RouterLink
  ],
  styleUrls: ["./validate-employee.component.scss"]
})
export class ValidateEmployeeComponent implements OnInit {
  faCircleLeft = faCircleLeft;
  faThumbsUp = faThumbsUp;
  faSadTear = faSadTear;
  currentStep: number = 0;
  msgText: string = "";
  page: string = "";

  constructor(
    private activatedRoute: ActivatedRoute,
    private collaboratorService: CollaboratorService) {
  }

  ngOnInit(): void {
    this.activatedRoute.queryParams
      .subscribe(params => {
        const uuid = params["uuid"];
        setTimeout(() => {
          this.collaboratorService.validar(uuid.trim())
            .pipe(catchError(err => {
              this.msgText = err.error.msg;
              this.page = err.error.page;
              this.currentStep = 2;
              throw err;
            })).subscribe(result => {
            if (result.ok) {
              this.msgText = result.msg;
              this.page = result.page;
              this.currentStep = 1;
            }
          });
        }, 3000);
      });
  }

}
