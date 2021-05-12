package orderRepairHandler

import (
	"encoding/json"
	"github.com/gorilla/mux"
	"net/http"
	"sgc-server/database/orderRepair"
)

// GetOrderRepair obtiene una orden de reparación filtrado por id.
func GetOrderRepair(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	orderRepairId := vars["id"]
	if len(orderRepairId) < 1 {
		http.Error(w, "Debe enviar el parámetro ID", http.StatusBadRequest)
		return
	}

	doc, err := orderRepair.GetOrderRepair(orderRepairId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar buscar el registro, "+err.Error(), http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(doc)
}
