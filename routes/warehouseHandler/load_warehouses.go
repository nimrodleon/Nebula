package warehouseHandler

import (
	"encoding/json"
	"net/http"
	"sgt-server/database/warehouse"
	"strconv"
)

// LoadWarehouses cargar almacenes.
func LoadWarehouses(w http.ResponseWriter, r *http.Request) {
	page := r.URL.Query().Get("page")
	search := r.URL.Query().Get("search")

	pagTemp, err := strconv.Atoi(page)
	if err != nil {
		http.Error(w, "Debe enviar el parámetro página como entero mayor a 0", http.StatusBadRequest)
		return
	}

	pag := int64(pagTemp)

	result, status := warehouse.FindWarehouses(pag, search)
	if status == false {
		http.Error(w, "Error al leer los almacenes", http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(result)
}
