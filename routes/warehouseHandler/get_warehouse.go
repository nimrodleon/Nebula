package warehouseHandler

import (
	"encoding/json"
	"github.com/gorilla/mux"
	"net/http"
	"sgt-server/database/warehouse"
)

// GetWarehouse retorna un almacén.
func GetWarehouse(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	warehouseId := vars["id"]
	if len(warehouseId) < 1 {
		http.Error(w, "Debe enviar el parámetro ID", http.StatusBadRequest)
		return
	}

	doc, err := warehouse.GetWarehouse(warehouseId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar buscar el registro, "+err.Error(), http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(doc)
}
