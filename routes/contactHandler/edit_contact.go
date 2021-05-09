package contactHandler

import (
	"encoding/json"
	"github.com/gorilla/mux"
	"net/http"
	"sgt-server/database/contact"
	"sgt-server/models"
)

// EditContact endpoint modificar contacto.
func EditContact(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	contactId := vars["id"]

	var doc models.Contact

	err := json.NewDecoder(r.Body).Decode(&doc)
	if err != nil {
		http.Error(w, "Datos Incorrectos "+err.Error(), http.StatusBadRequest)
		return
	}

	status, err := contact.UpdateContact(doc, contactId)
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
