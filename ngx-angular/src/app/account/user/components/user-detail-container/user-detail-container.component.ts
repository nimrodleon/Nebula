import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, RouterLink, RouterLinkActive } from '@angular/router';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faUser } from '@fortawesome/free-regular-svg-icons';
import { faCoins, faFolderOpen, faWarehouse } from '@fortawesome/free-solid-svg-icons';
import { AccountNavbarComponent } from 'app/common/navbars/account-navbar/account-navbar.component';
import { UserService } from '../../services';
import { User } from '../../interfaces';
import { UpperCasePipe } from '@angular/common';

@Component({
  selector: 'app-user-detail-container',
  standalone: true,
  imports: [
    RouterLink,
    RouterLinkActive,
    FaIconComponent,
    AccountNavbarComponent,
    UpperCasePipe
  ],
  templateUrl: './user-detail-container.component.html'
})
export class UserDetailContainerComponent implements OnInit {
  faFolderOpen = faFolderOpen;
  faUser = faUser;
  faCoins = faCoins;
  user: User = new User();
  private userService: UserService = inject(UserService);
  private activatedRoute: ActivatedRoute = inject(ActivatedRoute);

  ngOnInit(): void {
    this.activatedRoute.paramMap.subscribe(params => {
      const userId = params.get("userId") || "";
      this.userService.show(userId).subscribe(result => this.user = result);
    });
  }

}
