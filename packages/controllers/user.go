package controllers

import (
	"encoding/json"
	"fmt"
	"github.com/gorilla/mux"
	"log"
	"net/http"
	"os"
	"sgc-server/packages/jwt"
	"sgc-server/packages/middlew"
	"sgc-server/packages/models"
	"sgc-server/packages/repository"
	"strconv"
	"time"
)

func UserRouterHandler(router *mux.Router) {
	router.HandleFunc("/api/auth/login", middlew.CheckDb(UserLoginHandler)).Methods("POST")
	router.HandleFunc("/api/auth/register_super_user", middlew.CheckDb(RegisterSuperUserHandler)).Methods("POST")
	router.HandleFunc("/api/users", middlew.CheckDb(middlew.ValidateJWT(GetUsersHandler))).Methods("GET")
	router.HandleFunc("/api/users/{id}", middlew.CheckDb(middlew.ValidateJWT(GetUserHandler))).Methods("GET")
	router.HandleFunc("/api/users", middlew.CheckDb(middlew.ValidateJWT(AddUserHandler))).Methods("POST")
	router.HandleFunc("/api/users/{id}", middlew.CheckDb(middlew.ValidateJWT(EditUserHandler))).Methods("PUT")
	router.HandleFunc("/api/users/{id}", middlew.CheckDb(middlew.ValidateJWT(DeleteUserHandler))).Methods("DELETE")
	router.HandleFunc("/api/users/change_status/{id}/{status}", middlew.CheckDb(middlew.ValidateJWT(UserChangeStatusHandler))).Methods("PATCH")
	router.HandleFunc("/api/users/password_change/{id}", middlew.CheckDb(middlew.ValidateJWT(UserPasswordChangeHandler))).Methods("PATCH")
	router.HandleFunc("/api/users/select2/q", middlew.CheckDb(middlew.ValidateJWT(GetUsersWithSelect2Handler))).Methods("GET")
}

// UserLoginHandler realiza el login.
func UserLoginHandler(w http.ResponseWriter, r *http.Request) {
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

	doc, exist := repository.UserLoginIntent(t.UserName, t.Password)
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

// RegisterSuperUserHandler crea el usuario super o actualiza su contraseña.
func RegisterSuperUserHandler(w http.ResponseWriter, r *http.Request) {
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

	_, exist, userId := repository.CheckUserExist(doc.UserName)
	if exist == true {
		status, _ := repository.UserPasswordChange(doc.Password, userId)
		if status == false {
			http.Error(w, "No se ha logrado modificar la contraseña", http.StatusBadRequest)
			return
		}
		w.WriteHeader(http.StatusOK)
	} else {
		_, status, _ := repository.CreateUser(doc)
		if status == false {
			http.Error(w, "No se ha logrado insertar el usuario", http.StatusBadRequest)
			return
		}
		w.WriteHeader(http.StatusOK)
	}
}

// GetUsersHandler cargar usuarios.
func GetUsersHandler(w http.ResponseWriter, r *http.Request) {
	page := r.URL.Query().Get("page")
	search := r.URL.Query().Get("search")

	pagTemp, err := strconv.Atoi(page)
	if err != nil {
		http.Error(w, "Debe enviar el parámetro página como entero mayor a 0", http.StatusBadRequest)
		return
	}

	pag := int64(pagTemp)

	if !jwt.CheckSuperUserPermission() {
		http.Error(w, "Error no tiene permiso para esta operación", http.StatusBadRequest)
		return
	}

	result, status := repository.GetUsers(pag, search)
	if status == false {
		http.Error(w, "Error al leer los usuarios", http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(result)
}

// GetUserHandler retorna un usuario.
func GetUserHandler(w http.ResponseWriter, r *http.Request) {
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

	doc, err := repository.GetUser(userId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar buscar el registro, "+err.Error(), http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(doc)
}

// AddUserHandler agregar usuarios.
func AddUserHandler(w http.ResponseWriter, r *http.Request) {
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

	_, exist, _ := repository.CheckUserExist(doc.UserName)
	if exist == true {
		http.Error(w, "No se ha logrado agregar usuario", http.StatusBadRequest)
		return
	}

	objID, status, err := repository.AddUser(doc)
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

// EditUserHandler editar usuario
func EditUserHandler(w http.ResponseWriter, r *http.Request) {
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

	status, err := repository.UpdateUser(doc, userId)
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

// DeleteUserHandler borra un usuario.
func DeleteUserHandler(w http.ResponseWriter, r *http.Request) {
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

	_, err := repository.DeleteUser(userId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar borrar, "+err.Error(), http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
}

// UserChangeStatusHandler cambia estado de la cuenta.
func UserChangeStatusHandler(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	userId := vars["id"]
	userStatus, _ := strconv.ParseBool(vars["status"])

	if !jwt.CheckSuperUserPermission() {
		http.Error(w, "Error no tiene permiso para esta operación", http.StatusBadRequest)
		return
	}

	status, err := repository.ChangeStatusUserAccount(userId, userStatus)
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

// UserPasswordChangeHandler Cambia la contraseña del usuario.
func UserPasswordChangeHandler(w http.ResponseWriter, r *http.Request) {
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

	status, err := repository.UserPasswordChange(doc.Password, userId)
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

// GetUsersWithSelect2Handler buscar usuarios con select2.
func GetUsersWithSelect2Handler(w http.ResponseWriter, r *http.Request) {
	search := r.URL.Query().Get("term")

	var data models.Select2

	result, status := repository.GetActiveUsers(search)
	if status == false {
		http.Error(w, "Error al leer los usuarios", http.StatusBadRequest)
		return
	}

	for i := 0; i < len(result); i++ {
		data.Results = append(data.Results, models.Select2Item{ID: result[i].ID.Hex(), Text: result[i].FullName})
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(data)
}
