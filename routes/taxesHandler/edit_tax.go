package taxesHandler

import (
	"encoding/json"
	"github.com/gorilla/mux"
	"net/http"
	"sgt-server/database/taxes"
	"sgt-server/models"
)

// EditTax editar impuesto.
func EditTax(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	taxId := vars["id"]

	var doc models.Tax

	err := json.NewDecoder(r.Body).Decode(&doc)
	if err != nil {
		http.Error(w, "Datos Incorrectos "+err.Error(), http.StatusBadRequest)
		return
	}

	status, err := taxes.UpdateTax(doc, taxId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar modificar el registro. "+err.Error(), http.StatusBadRequest)
		return
	}
	if status == false {
		http.Error(w, "No se ha logrado modificar el registro", http.StatusBadRequest)
		return
	}

	w.WriteHeader(http.StatusCreated)
}
