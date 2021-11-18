import {Component, OnInit} from '@angular/core';
import {faPlus, faSearch, faSignOutAlt, faTrashAlt} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-history-list',
  templateUrl: './history-list.component.html',
  styleUrls: ['./history-list.component.scss']
})
export class HistoryListComponent implements OnInit {
  faSearch = faSearch;
  faPlus = faPlus;
  faSignOutAlt = faSignOutAlt;
  faTrashAlt = faTrashAlt;

  constructor() {
  }

  ngOnInit(): void {
  }

}
