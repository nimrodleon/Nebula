package contactHandler

import (
	"github.com/gorilla/mux"
	"net/http"
	"sgc-server/database/contact"
	"sgc-server/routes/jwt"
)

// DeleteContact endpoint borrar contacto.
func DeleteContact(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	contactId := vars["id"]
	if len(contactId) < 1 {
		http.Error(w, "Debe enviar el parámetro ID", http.StatusBadRequest)
		return
	}

	if !jwt.CheckDeletePermission() {
		http.Error(w, "Error no tiene permiso para eliminar este documento", http.StatusBadRequest)
		return
	}

	_, err := contact.DeleteContact(contactId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar borrar, "+err.Error(), http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
}
