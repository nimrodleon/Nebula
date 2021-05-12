package orderRepairHandler

import (
	"github.com/gorilla/mux"
	"net/http"
	"sgc-server/database/orderRepair"
	"sgc-server/routes/jwt"
)

// DeleteOrderRepair borra una orden de reparación.
func DeleteOrderRepair(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	orderRepairId := vars["id"]
	if len(orderRepairId) < 1 {
		http.Error(w, "Debe enviar el parámetro ID", http.StatusBadRequest)
		return
	}

	if !jwt.CheckDeletePermission() {
		http.Error(w, "Error no tiene permiso para eliminar este documento", http.StatusBadRequest)
		return
	}

	_, err := orderRepair.DeleteOrderRepair(orderRepairId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar borrar, "+err.Error(), http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
}
