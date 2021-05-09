package userHandler

import (
	"github.com/gorilla/mux"
	"net/http"
	"sgt-server/database/user"
	"sgt-server/routes/jwt"
	"strconv"
)

// ChangeStatus cambia estado de la cuenta.
func ChangeStatus(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	userId := vars["id"]
	userStatus, _ := strconv.ParseBool(vars["status"])

	if !jwt.CheckSuperUserPermission() {
		http.Error(w, "Error no tiene permiso para esta operación", http.StatusBadRequest)
		return
	}

	status, err := user.ChangeStatusUserAccount(userId, userStatus)
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
