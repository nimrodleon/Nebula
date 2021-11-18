import { Component, OnInit } from '@angular/core';
import {faPlus, faSearch, faSignOutAlt, faTrashAlt} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-transfer-list',
  templateUrl: './transfer-list.component.html',
  styleUrls: ['./transfer-list.component.scss']
})
export class TransferListComponent implements OnInit {
  faSearch = faSearch;
  faPlus = faPlus;
  faSignOutAlt = faSignOutAlt;
  faTrashAlt = faTrashAlt;

  constructor() { }

  ngOnInit(): void {
  }

}
