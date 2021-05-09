package deviceTypeHandler

import (
	"github.com/gorilla/mux"
	"net/http"
	"sgt-server/database/deviceType"
	"sgt-server/routes/jwt"
)

// DeleteDeviceType borra un tipo de equipo.
func DeleteDeviceType(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	deviceTypeId := vars["id"]
	if len(deviceTypeId) < 1 {
		http.Error(w, "Debe enviar el parámetro ID", http.StatusBadRequest)
		return
	}

	if !jwt.CheckDeletePermission() {
		http.Error(w, "Error no tiene permiso para eliminar este documento", http.StatusBadRequest)
		return
	}

	_, err := deviceType.DeleteDeviceType(deviceTypeId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar borrar, "+err.Error(), http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
}
