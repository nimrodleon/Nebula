import {environment} from "environments/environment";
import {Injector} from "@angular/core";
import {UserDataService} from "../user-data.service";

declare const jQuery: any;
const appURL: string = environment.applicationUrl;
let injector: Injector;

export const initializeSelect2Injector = (injectorInstance: Injector) => {
  injector = injectorInstance;
};

export const select2Contactos = (element: string, dropdownParent: any = undefined, companyId: string = "") => {
  if (companyId === "") {
    const _userDataService: UserDataService = injector.get(UserDataService);
    companyId = _userDataService.companyId;
  }
  return jQuery(element).select2({
    theme: "bootstrap-5",
    placeholder: "BUSCAR CONTACTO",
    dropdownParent: dropdownParent ? jQuery(dropdownParent) : undefined,
    ajax: {
      url: appURL + "contacts/" + companyId + "/Contact/Select2",
      headers: {
        Authorization: "Bearer " + localStorage.getItem("token")
      },
      dataType: "json",
    }
  });
};

export const select2Productos = (element: string, dropdownParent: any = undefined, companyId: string = "") => {
  if (companyId === "") {
    const _userDataService: UserDataService = injector.get(UserDataService);
    companyId = _userDataService.companyId;
  }
  return jQuery(element).select2({
    theme: "bootstrap-5",
    placeholder: "BUSCAR PRODUCTOS/SERVICIOS",
    dropdownParent: dropdownParent,
    ajax: {
      method: "GET",
      url: appURL + "products/" + companyId + "/Product/Select2",
      headers: {
        Authorization: "Bearer " + localStorage.getItem("token")
      },
      dataType: "json",
    },
  });
};

export const select2Category = (element: string, dropdownParent: any = undefined, companyId: string = "") => {
  if (companyId === "") {
    const _userDataService: UserDataService = injector.get(UserDataService);
    companyId = _userDataService.companyId;
  }
  return jQuery(element).select2({
    theme: "bootstrap-5",
    placeholder: "BUSCAR CATEGORÍA",
    dropdownParent: dropdownParent,
    ajax: {
      method: "GET",
      url: appURL + "products/" + companyId + "/Category/Select2",
      headers: {
        Authorization: "Bearer " + localStorage.getItem("token")
      },
      dataType: "json",
    }
  });
};
