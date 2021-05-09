package userHandler

import (
	"encoding/json"
	"github.com/gorilla/mux"
	"net/http"
	"sgc-server/database/user"
	"sgc-server/routes/jwt"
)

// GetUser retorna un usuario.
func GetUser(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	userId := vars["id"]
	if len(userId) < 1 {
		http.Error(w, "Debe enviar el parámetro ID", http.StatusBadRequest)
		return
	}

	if !jwt.CheckSuperUserPermission() {
		http.Error(w, "Error no tiene permiso para esta operación", http.StatusBadRequest)
		return
	}

	doc, err := user.GetUser(userId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar buscar el registro, "+err.Error(), http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(doc)
}
