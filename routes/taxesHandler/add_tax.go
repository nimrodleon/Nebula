package taxesHandler

import (
	"encoding/json"
	"fmt"
	"net/http"
	"sgt-server/database/taxes"
	"sgt-server/models"
)

// AddTax agrega un registro a la DB.
func AddTax(w http.ResponseWriter, r *http.Request) {
	var doc models.Tax
	err := json.NewDecoder(r.Body).Decode(&doc)
	if err != nil {
		http.Error(w, "Error en los datos recibidos "+err.Error(), http.StatusBadRequest)
		return
	}

	objID, status, err := taxes.AddTax(doc)
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
