package taxesHandler

import (
	"encoding/json"
	"github.com/gorilla/mux"
	"net/http"
	"sgt-server/database/taxes"
)

// GetTax retorna un impuesto en especifico.
func GetTax(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	taxId := vars["id"]
	if len(taxId) < 1 {
		http.Error(w, "Debe enviar el parámetro ID", http.StatusBadRequest)
		return
	}

	doc, err := taxes.GetTax(taxId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar buscar el registro, "+err.Error(), http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(doc)
}
