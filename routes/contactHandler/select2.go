package contactHandler

import (
	"encoding/json"
	"net/http"
	"sgt-server/database/contact"
	"sgt-server/models"
)

// FindContactWithSelect2 buscar contactos con select2.
func FindContactWithSelect2(w http.ResponseWriter, r *http.Request) {
	search := r.URL.Query().Get("term")

	var data models.Select2

	result, status := contact.FindContacts(1, search)
	if status == false {
		http.Error(w, "Error al leer los contactos", http.StatusBadRequest)
		return
	}

	for i := 0; i < len(result); i++ {
		data.Results = append(data.Results, models.Select2Item{ID: result[i].ID.Hex(), Text: result[i].FullName})
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(data)
}
