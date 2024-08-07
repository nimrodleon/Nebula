import {Component, EventEmitter, OnInit, Output} from "@angular/core";
import {select2Contactos} from "app/common/interfaces";
import {Contact} from "app/contact/interfaces";
import {faUserAlt} from "@fortawesome/free-solid-svg-icons";
import {FaIconComponent} from "@fortawesome/angular-fontawesome";

@Component({
  selector: "app-search-contact-modal",
  standalone: true,
  imports: [
    FaIconComponent
  ],
  templateUrl: "./search-contact-modal.component.html"
})
export class SearchContactModalComponent implements OnInit {
  faUserAlt = faUserAlt;
  contact: Contact = new Contact();
  @Output()
  responseData: EventEmitter<Contact> = new EventEmitter<Contact>();

  ngOnInit(): void {
    const contacto = select2Contactos("#contacto", "#searchContactModal")
      .on("select2:select", (e: any) => {
        const data = e.params.data;
        this.contact = {...data};
      });
    const myModalEl: any = document.getElementById("searchContactModal");
    myModalEl.addEventListener("hidden.bs.modal", () => {
      contacto.val(null).trigger("change");
      this.contact = new Contact();
    });
  }

  public seleccionarContacto(): void {
    this.responseData.emit({...this.contact});
  }
}
