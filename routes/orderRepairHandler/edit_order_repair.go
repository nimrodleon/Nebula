package orderRepairHandler

import (
	"encoding/json"
	"github.com/gorilla/mux"
	"net/http"
	"sgc-server/database/orderRepair"
	"sgc-server/models"
)

// EditOrderRepair actualiza la información de una orden de reparación.
func EditOrderRepair(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	orderRepairId := vars["id"]

	var doc models.OrderRepair

	err := json.NewDecoder(r.Body).Decode(&doc)
	if err != nil {
		http.Error(w, "Datos Incorrectos "+err.Error(), http.StatusBadRequest)
		return
	}

	status, err := orderRepair.UpdateOrderRepair(doc, orderRepairId)
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
