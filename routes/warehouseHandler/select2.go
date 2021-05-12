package warehouseHandler

import (
	"encoding/json"
	"github.com/gorilla/mux"
	"net/http"
	"sgc-server/database/warehouse"
	"sgc-server/models"
)

// FindWarehousesWithSelect2 buscar almacenes desde select2.
func FindWarehousesWithSelect2(w http.ResponseWriter, r *http.Request) {
	search := r.URL.Query().Get("term")
	vars := mux.Vars(r)
	typeWarehouse := vars["type"]

	if len(typeWarehouse) < 1 {
		http.Error(w, "Debe enviar el parámetro ID", http.StatusBadRequest)
		return
	}

	var data models.Select2

	result, status := warehouse.GetWarehousesWithType(typeWarehouse, search)
	if status == false {
		http.Error(w, "Error al leer los contactos", http.StatusBadRequest)
		return
	}

	for i := 0; i < len(result); i++ {
		data.Results = append(data.Results, models.Select2Item{ID: result[i].ID.Hex(), Text: result[i].Name})
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(data)
}
