package userHandler

import (
	"encoding/json"
	"fmt"
	"net/http"
	"sgc-server/database/auth"
	"sgc-server/database/user"
	"sgc-server/models"
	"sgc-server/routes/jwt"
)

// AddUser agregar usuarios.
func AddUser(w http.ResponseWriter, r *http.Request) {
	var doc models.User
	err := json.NewDecoder(r.Body).Decode(&doc)
	if err != nil {
		http.Error(w, "Error en los datos recibidos "+err.Error(), http.StatusBadRequest)
		return
	}

	if !jwt.CheckSuperUserPermission() {
		http.Error(w, "Error no tiene permiso para esta operación", http.StatusBadRequest)
		return
	}

	_, exist, _ := auth.CheckUserExist(doc.UserName)
	if exist == true {
		http.Error(w, "No se ha logrado agregar usuario", http.StatusBadRequest)
		return
	}

	objID, status, err := user.AddUser(doc)
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
