import {Component, EventEmitter, Input, Output} from "@angular/core";
import {faTrashAlt} from "@fortawesome/free-solid-svg-icons";
import {ReceivableDetailDataModal} from "../../interfaces";
import {ReceivableService} from "../../services";
import {accessDenied, deleteConfirm} from "app/common/interfaces";
import {UserDataService} from "app/common/user-data.service";
import _ from "lodash";
import {CurrencyPipe, DatePipe, NgForOf, NgIf} from "@angular/common";
import {RouterLink} from "@angular/router";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";

@Component({
  selector: "app-abono-detail",
  standalone: true,
  imports: [
    DatePipe,
    NgForOf,
    NgIf,
    RouterLink,
    CurrencyPipe,
    FaIconComponent
  ],
  templateUrl: "./abono-detail.component.html"
})
export class AbonoDetailComponent {
  faTrashAlt = faTrashAlt;
  @Input()
  cargoDetail: ReceivableDetailDataModal = new ReceivableDetailDataModal();
  @Output()
  responseData = new EventEmitter<boolean>();

  constructor(
    private userDataService: UserDataService,
    private receivableService: ReceivableService) {
  }

  public deleteAbono(id: string): void {
    if (!this.userDataService.canDelete()) {
      accessDenied().then(() => console.log("accessDenied"));
    } else {
      deleteConfirm().then(result => {
        if (result.isConfirmed) {
          this.receivableService.delete(id).subscribe(result => {
            this.responseData.emit(true);
            this.cargoDetail.abonos = _.filter(this.cargoDetail.abonos, item => item.id !== result.id);
          });
        }
      });
    }
  }

}
