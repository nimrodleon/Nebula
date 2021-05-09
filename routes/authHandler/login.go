package authHandler

import (
	"encoding/json"
	"net/http"
	"sgc-server/database/auth"
	"sgc-server/models"
	"sgc-server/routes/jwt"
	"time"
)

// Login realiza el login.
func Login(w http.ResponseWriter, r *http.Request) {
	w.Header().Add("Content-Type", "application/json")

	var t models.User

	err := json.NewDecoder(r.Body).Decode(&t)
	if err != nil {
		http.Error(w, "Usuario y/o Contraseña inválidos "+err.Error(), http.StatusBadRequest)
		return
	}
	if len(t.UserName) == 0 {
		http.Error(w, "El Usuario es requerido", http.StatusBadRequest)
		return
	}

	doc, exist := auth.LoginIntent(t.UserName, t.Password)
	if exist == false {
		http.Error(w, "Usuario y/o Contraseña inválidos", http.StatusBadRequest)
		return
	}

	jwtKey, err := jwt.GenerateJWT(doc)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar general el Token correspondiente "+err.Error(), http.StatusBadRequest)
		return
	}

	resp := models.ResponseLogin{
		Token: jwtKey,
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(resp)

	expirationTime := time.Now().Add(24 * time.Hour)
	http.SetCookie(w, &http.Cookie{
		Name:    "token",
		Value:   jwtKey,
		Expires: expirationTime,
	})
}
