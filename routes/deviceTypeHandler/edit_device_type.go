package deviceTypeHandler

import (
	"encoding/json"
	"github.com/gorilla/mux"
	"net/http"
	"sgc-server/database/deviceType"
	"sgc-server/models"
)

// EditDeviceType editar tipo de equipo.
func EditDeviceType(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	deviceTypeId := vars["id"]

	var doc models.DeviceType

	err := json.NewDecoder(r.Body).Decode(&doc)
	if err != nil {
		http.Error(w, "Datos Incorrectos "+err.Error(), http.StatusBadRequest)
		return
	}

	status, err := deviceType.UpdateDeviceType(doc, deviceTypeId)
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
