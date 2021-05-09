package deviceTypeHandler

import (
	"encoding/json"
	"github.com/gorilla/mux"
	"net/http"
	"sgc-server/database/deviceType"
)

// GetDeviceType retorna un tipo de equipo.
func GetDeviceType(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	deviceTypeId := vars["id"]
	if len(deviceTypeId) < 1 {
		http.Error(w, "Debe enviar el parámetro ID", http.StatusBadRequest)
		return
	}

	doc, err := deviceType.GetDeviceType(deviceTypeId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar buscar el registro, "+err.Error(), http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(doc)
}
