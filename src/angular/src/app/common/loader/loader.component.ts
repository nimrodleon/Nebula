import {Component, Input} from "@angular/core";
import {NgIf} from "@angular/common";

@Component({
  selector: "app-loader",
  templateUrl: "./loader.component.html",
  standalone: true,
  imports: [
    NgIf
  ],
  styleUrls: ["./loader.component.scss"]
})
export class LoaderComponent {
  @Input()
  loading: boolean = false;
}
