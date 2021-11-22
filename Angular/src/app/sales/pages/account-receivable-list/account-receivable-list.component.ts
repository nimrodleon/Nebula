import {Component, OnInit} from '@angular/core';
import {faCoins, faSearch, faTrashAlt} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-account-receivable-list',
  templateUrl: './account-receivable-list.component.html',
  styleUrls: ['./account-receivable-list.component.scss']
})
export class AccountReceivableListComponent implements OnInit {
  faSearch = faSearch;
  faTrashAlt = faTrashAlt;
  faCoins = faCoins;

  constructor() {
  }

  ngOnInit(): void {
  }

}
