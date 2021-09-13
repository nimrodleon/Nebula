import {Component, OnInit} from '@angular/core';
import {
  faCog,
  faLock, faMinusCircle,
  faPlus,
  faPrint, faSearch,
  faSignOutAlt,
  faSyncAlt,
  faTimes,
  faTimesCircle,
  faTrashAlt
} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-cash-detail',
  templateUrl: './cash-detail.component.html',
  styleUrls: ['./cash-detail.component.scss']
})
export class CashDetailComponent implements OnInit {
  faSyncAlt = faSyncAlt;
  faPlus = faPlus;
  faTrashAlt = faTrashAlt;
  faSignOutAlt = faSignOutAlt;
  faLock = faLock;
  faPrint = faPrint;
  faMinusCircle = faMinusCircle;
  faSearch = faSearch;
  faCog=faCog;

  constructor() {
  }

  ngOnInit(): void {
  }

}
