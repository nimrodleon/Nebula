package contactHandler

import (
	"encoding/json"
	"github.com/gorilla/mux"
	"net/http"
	"sgt-server/database/contact"
)

// GetContact devuelve un contacto por id.
func GetContact(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	contactId := vars["id"]
	if len(contactId) < 1 {
		http.Error(w, "Debe enviar el parámetro ID", http.StatusBadRequest)
		return
	}

	doc, err := contact.GetContact(contactId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar buscar el registro, "+err.Error(), http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(doc)
}
