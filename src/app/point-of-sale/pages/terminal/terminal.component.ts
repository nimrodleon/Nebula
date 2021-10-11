import {Component, OnInit} from '@angular/core';
import {
  faCheck,
  faCoins,
  faLock,
  faMinus,
  faPlus,
  faSearch,
  faTrashAlt,
  faUserCircle
} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-terminal',
  templateUrl: './terminal.component.html',
  styleUrls: ['./terminal.component.scss']
})
export class TerminalComponent implements OnInit {
  faUserCircle = faUserCircle;
  faPlus = faPlus;
  faSearch = faSearch;
  faLock = faLock;
  faCoins = faCoins;
  faCheck = faCheck;
  faTrashAlt = faTrashAlt;
  faMinus = faMinus;

  constructor() {
  }

  ngOnInit(): void {
  }

}
