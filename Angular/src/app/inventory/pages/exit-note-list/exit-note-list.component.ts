import { Component, OnInit } from '@angular/core';
import {faPlus, faSearch, faSignOutAlt, faTrashAlt} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-exit-note-list',
  templateUrl: './exit-note-list.component.html',
  styleUrls: ['./exit-note-list.component.scss']
})
export class ExitNoteListComponent implements OnInit {
  faSearch = faSearch;
  faPlus = faPlus;
  faSignOutAlt = faSignOutAlt;
  faTrashAlt = faTrashAlt;

  constructor() { }

  ngOnInit(): void {
  }

}
