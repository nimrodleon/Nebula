package warehouseHandler

import (
	"encoding/json"
	"github.com/gorilla/mux"
	"net/http"
	"sgc-server/database/warehouse"
	"sgc-server/models"
)

// EditWarehouse editar almacén.
func EditWarehouse(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	warehouseId := vars["id"]

	var doc models.Warehouse

	err := json.NewDecoder(r.Body).Decode(&doc)
	if err != nil {
		http.Error(w, "Datos Incorrectos "+err.Error(), http.StatusBadRequest)
		return
	}

	status, err := warehouse.UpdateWarehouse(doc, warehouseId)
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
