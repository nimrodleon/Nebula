import {Component, OnInit} from '@angular/core';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {

  constructor() {
  }

  ngOnInit(): void {
    // class por defecto mainMenu.
    const mainMenu: any = document.getElementById('MainMenu');
    mainMenu.classList.value = localStorage.getItem('classMainMenu');
  }

}
