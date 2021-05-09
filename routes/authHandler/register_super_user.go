package authHandler

import (
	"log"
	"net/http"
	"os"
	"sgc-server/database/auth"
	"sgc-server/models"
)

// RegisterSuperUser crea el usuario super o actualiza su contraseña.
func RegisterSuperUser(w http.ResponseWriter, r *http.Request) {
	w.Header().Add("Content-Type", "application/json")

	passwd, defined := os.LookupEnv("SGT_SERVER_SUPER_PASSWORD")
	log.Println(passwd, defined)
	if defined == false {
		http.Error(w, "No está definida la variable SGT_SERVER_SUPER_PASSWORD", http.StatusInternalServerError)
		return
	}

	var doc models.User
	doc.FullName = "SUPER_USER"
	doc.UserName = "super"
	doc.Password = passwd
	doc.Permission = "ROLE_SUPER"
	doc.Suspended = false

	_, exist, userId := auth.CheckUserExist(doc.UserName)
	if exist == true {
		status, _ := auth.PasswordChange(doc.Password, userId)
		if status == false {
			http.Error(w, "No se ha logrado modificar la contraseña", http.StatusBadRequest)
			return
		}
		w.WriteHeader(http.StatusOK)
	} else {
		_, status, _ := auth.CreateUser(doc)
		if status == false {
			http.Error(w, "No se ha logrado insertar el usuario", http.StatusBadRequest)
			return
		}
		w.WriteHeader(http.StatusOK)
	}
}
