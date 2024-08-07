import {Component} from "@angular/core";
import {FormType} from "app/common/interfaces";
import {CompanyFormComponent} from "../../components/company-form/company-form.component";
import {AccountContainerComponent} from "app/common/containers/account-container/account-container.component";

@Component({
  selector: "app-company-edit",
  standalone: true,
  imports: [
    CompanyFormComponent,
    AccountContainerComponent
  ],
  templateUrl: "./company-edit.component.html"
})
export class CompanyEditComponent {
  protected readonly FormType = FormType;
}
