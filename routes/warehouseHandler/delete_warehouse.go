package warehouseHandler

import (
	"github.com/gorilla/mux"
	"net/http"
	"sgc-server/database/warehouse"
	"sgc-server/routes/jwt"
)

// DeleteWarehouse borrar almacenes.
func DeleteWarehouse(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	warehouseId := vars["id"]
	if len(warehouseId) < 1 {
		http.Error(w, "Debe enviar el parámetro ID", http.StatusBadRequest)
		return
	}

	if !jwt.CheckDeletePermission() {
		http.Error(w, "Error no tiene permiso para eliminar este documento", http.StatusBadRequest)
		return
	}

	_, err := warehouse.DeleteWarehouse(warehouseId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar borrar, "+err.Error(), http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
}
