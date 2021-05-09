package contactHandler

import (
	"encoding/json"
	"fmt"
	"net/http"
	"sgt-server/database/contact"
	"sgt-server/models"
)

// AddContact endpoint registrar contacto.
func AddContact(w http.ResponseWriter, r *http.Request) {
	var doc models.Contact
	err := json.NewDecoder(r.Body).Decode(&doc)
	if err != nil {
		http.Error(w, "Error en los datos recibidos "+err.Error(), http.StatusBadRequest)
		return
	}

	objID, status, err := contact.AddContact(doc)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar realizar el registro "+err.Error(), http.StatusBadRequest)
		return
	}
	if status == false {
		http.Error(w, "No se ha logrado insertar el registro", http.StatusBadRequest)
		return
	}
	w.WriteHeader(http.StatusCreated)
	_, _ = fmt.Fprint(w, objID)
}
