import {Component, inject, Input, OnInit} from "@angular/core";
import {ActivatedRoute, RouterLink, RouterLinkActive} from "@angular/router";
import {faCoins, faCreditCard, faFolderOpen, faRightFromBracket} from "@fortawesome/free-solid-svg-icons";
import {ContactDetailService} from "../../services";
import {UserDataService} from "app/common/user-data.service";
import {ContactContainerComponent} from "app/common/containers/contact-container/contact-container.component";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";

@Component({
  selector: "app-contact-detail-container",
  standalone: true,
  imports: [
    ContactContainerComponent,
    RouterLink,
    FaIconComponent,
    RouterLinkActive
  ],
  templateUrl: "./contact-detail-container.component.html"
})
export class ContactDetailContainerComponent implements OnInit {
  faFolderOpen = faFolderOpen;
  faCreditCard = faCreditCard;
  faCoins = faCoins;
  faRightFromBracket = faRightFromBracket;
  userDataService: UserDataService = inject(UserDataService);

  @Input()
  type: string = "receivable";

  constructor(
    private activatedRoute: ActivatedRoute,
    private contactDetailService: ContactDetailService) {
  }

  ngOnInit(): void {
    const id: string = this.activatedRoute.snapshot.params["id"];
    this.activatedRoute.queryParams.subscribe(params => {
      this.contactDetailService.getDetail(id, this.type);
    });
  }

  public get contact() {
    return this.contactDetailService.contact;
  }

}
