package userHandler

import (
	"encoding/json"
	"net/http"
	"sgc-server/database/user"
	"sgc-server/models"
)

// FindUsersWithSelect2 buscar usuarios con select2.
func FindUsersWithSelect2(w http.ResponseWriter, r *http.Request) {
	search := r.URL.Query().Get("term")

	var data models.Select2

	result, status := user.GetActiveUsers(search)
	if status == false {
		http.Error(w, "Error al leer los usuarios", http.StatusBadRequest)
		return
	}

	for i := 0; i < len(result); i++ {
		data.Results = append(data.Results, models.Select2Item{ID: result[i].ID.Hex(), Text: result[i].FullName})
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(data)
}
