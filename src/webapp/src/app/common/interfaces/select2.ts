import {environment} from "environments/environment";

declare const jQuery: any;
const appURL: string = environment.applicationUrl;

export const select2Contactos = (element: string, dropdownParent: any = undefined) => {
  return jQuery(element).select2({
    theme: "bootstrap-5",
    placeholder: "BUSCAR CONTACTO",
    dropdownParent: dropdownParent ? jQuery(dropdownParent) : undefined,
    ajax: {
      url: appURL + "contacts/Contact/Select2",
      headers: {
        Authorization: "Bearer " + localStorage.getItem("token")
      },
      dataType: "json",
    }
  });
};

export const select2Productos = (element: string, dropdownParent: any = undefined) => {
  return jQuery(element).select2({
    theme: "bootstrap-5",
    placeholder: "BUSCAR PRODUCTOS/SERVICIOS",
    dropdownParent: dropdownParent,
    ajax: {
      method: "GET",
      url: appURL + "products/Product/Select2",
      headers: {
        Authorization: "Bearer " + localStorage.getItem("token")
      },
      dataType: "json",
    },
  });
};

export const select2Category = (element: string, dropdownParent: any = undefined) => {
  return jQuery(element).select2({
    theme: "bootstrap-5",
    placeholder: "BUSCAR CATEGORÍA",
    dropdownParent: dropdownParent,
    ajax: {
      method: "GET",
      url: appURL + "products/Category/Select2",
      headers: {
        Authorization: "Bearer " + localStorage.getItem("token")
      },
      dataType: "json",
    }
  });
};
