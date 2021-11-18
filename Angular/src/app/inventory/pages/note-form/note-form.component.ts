import {Component, OnInit} from '@angular/core';
import {faArrowLeft, faEdit, faPlus, faSave, faTrashAlt} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-note-form',
  templateUrl: './note-form.component.html',
  styleUrls: ['./note-form.component.scss']
})
export class NoteFormComponent implements OnInit {
  faArrowLeft = faArrowLeft;
  faPlus = faPlus;
  faEdit = faEdit;
  faTrashAlt = faTrashAlt;
  faSave = faSave;

  constructor() {
  }

  ngOnInit(): void {
  }

}
