import {Component, OnInit} from '@angular/core';
import {faCheck, faCoins, faLock, faPlus, faSearch, faUserCircle} from '@fortawesome/free-solid-svg-icons';

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

  constructor() {
  }

  ngOnInit(): void {
  }

}
