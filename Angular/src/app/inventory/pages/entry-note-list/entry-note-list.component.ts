import {Component, OnInit} from '@angular/core';
import {faIdCardAlt, faPlus, faSearch, faSignOutAlt, faTrashAlt} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-entry-note-list',
  templateUrl: './entry-note-list.component.html',
  styleUrls: ['./entry-note-list.component.scss']
})
export class EntryNoteListComponent implements OnInit {
  faSearch = faSearch;
  faPlus = faPlus;
  faSignOutAlt = faSignOutAlt;
  faTrashAlt = faTrashAlt;
  faIdCardAlt = faIdCardAlt;

  constructor() {
  }

  ngOnInit(): void {
  }

}
