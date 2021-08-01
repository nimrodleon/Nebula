import {Component, OnInit} from '@angular/core';
import {faLock, faSignInAlt, faUser} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  faUser = faUser;
  faLock = faLock;
  faSignInAlt = faSignInAlt;

  constructor() {
  }

  ngOnInit(): void {
  }

}
