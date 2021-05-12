package deviceTypeHandler

import (
	"encoding/json"
	"net/http"
	"sgc-server/database/deviceType"
	"sgc-server/models"
)

// FindDeviceTypeWithSelect2 buscar tipos de equipo con select2.
func FindDeviceTypeWithSelect2(w http.ResponseWriter, r *http.Request) {
	search := r.URL.Query().Get("term")

	var data models.Select2

	result, status := deviceType.FindDeviceTypes(1, search)
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
