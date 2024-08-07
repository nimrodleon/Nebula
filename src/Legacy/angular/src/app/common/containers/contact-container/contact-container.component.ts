import {Component, OnInit} from "@angular/core";
import {ActivatedRoute} from "@angular/router";
import {ContactNavbarComponent} from "../../navbars/contact-navbar/contact-navbar.component";

@Component({
  selector: "app-contact-container",
  standalone: true,
  imports: [
    ContactNavbarComponent
  ],
  templateUrl: "./contact-container.component.html"
})
export class ContactContainerComponent implements OnInit {
  companyId: string = "";

  constructor(private route: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
    });
  }
}
