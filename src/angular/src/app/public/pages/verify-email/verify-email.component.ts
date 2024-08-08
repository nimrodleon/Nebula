import {Component, OnInit} from "@angular/core";
import {ActivatedRoute, Router} from "@angular/router";
import {UserService} from "app/account/user/services";
import {NgIf} from "@angular/common";

@Component({
  selector: "app-verify-email",
  templateUrl: "./verify-email.component.html",
  styleUrls: ["./verify-email.component.scss"],
  standalone: true,
  imports: [
    NgIf
  ]
})
export class VerifyEmailComponent implements OnInit {
  verificado: boolean = false;
  messageResult: string = "";

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private userService: UserService) {
  }

  ngOnInit(): void {
    this.activatedRoute.queryParams
      .subscribe(params => {
        const token = params["token"];
        setTimeout(() => {
          this.userService.verifyEmail(token)
            .subscribe(result => {
              this.verificado = result.ok;
              this.messageResult = result.msg;
              setTimeout(() => {
                this.router.navigate(["/login"])
                  .then(() => console.log("redirect"));
              }, 3000);
            });
        }, 3000);
      });
  }

}
