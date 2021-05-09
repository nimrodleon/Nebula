package userHandler

import (
	"encoding/json"
	"github.com/gorilla/mux"
	"net/http"
	"sgt-server/database/auth"
	"sgt-server/models"
	"sgt-server/routes/jwt"
)

// PasswordChange Cambia la contraseña del usuario.
func PasswordChange(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	userId := vars["id"]

	var doc models.User

	err := json.NewDecoder(r.Body).Decode(&doc)
	if err != nil {
		http.Error(w, "Datos Incorrectos "+err.Error(), http.StatusBadRequest)
		return
	}

	if !jwt.CheckSuperUserPermission() {
		http.Error(w, "Error no tiene permiso para esta operación", http.StatusBadRequest)
		return
	}

	status, err := auth.PasswordChange(doc.Password, userId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar modificar el registro. "+err.Error(), http.StatusBadRequest)
		return
	}
	if status == false {
		http.Error(w, "No se ha logrado modificar la contraseña", http.StatusBadRequest)
		return
	}

	w.WriteHeader(http.StatusOK)
}
