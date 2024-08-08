import {Component, OnInit} from "@angular/core";
import {ActivatedRoute} from "@angular/router";
import {TallerNavbarComponent} from "../../navbars/taller-navbar/taller-navbar.component";

@Component({
  selector: "app-taller-container",
  standalone: true,
  imports: [
    TallerNavbarComponent
  ],
  templateUrl: "./taller-container.component.html"
})
export class TallerContainerComponent implements OnInit {
  companyId: string = "";

  constructor(private route: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.companyId = params.get("companyId") || "";
    });
  }
}
