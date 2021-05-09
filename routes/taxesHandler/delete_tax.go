package taxesHandler

import (
	"github.com/gorilla/mux"
	"net/http"
	"sgt-server/database/taxes"
	"sgt-server/routes/jwt"
)

// DeleteTax borra un impuesto.
func DeleteTax(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	taxId := vars["id"]
	if len(taxId) < 1 {
		http.Error(w, "Debe enviar el parámetro ID", http.StatusBadRequest)
		return
	}

	if !jwt.CheckDeletePermission() {
		http.Error(w, "Error no tiene permiso para eliminar este documento", http.StatusBadRequest)
		return
	}

	_, err := taxes.DeleteTax(taxId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar borrar, "+err.Error(), http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
}
