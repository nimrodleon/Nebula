import {inject, Injectable} from "@angular/core";
import {UserDataService} from "./user-data.service";
import {AuthService} from "../account/user/services";

@Injectable({
  providedIn: "root"
})
export class AppInitializerService {
  private authService: AuthService = inject(AuthService);
  private userDataService: UserDataService = inject(UserDataService);

  initializeApp(): Promise<any> {
    return new Promise((resolve, reject) => {
      if (this.authService.getToken() !== "") {
        this.userDataService.cargarData();
      }
      resolve(null);
    });
  }

}
