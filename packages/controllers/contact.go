package controllers

import (
	"encoding/json"
	"fmt"
	"github.com/gorilla/mux"
	"net/http"
	"sgc-server/packages/jwt"
	"sgc-server/packages/middlew"
	"sgc-server/packages/models"
	"sgc-server/packages/services"
	"strconv"
)

func ContactRouterHandler(router *mux.Router) {
	router.HandleFunc("/api/contacts",
		middlew.CheckDb(middlew.ValidateJWT(GetContactsHandler))).Methods("GET")
	router.HandleFunc("/api/contacts/{id}",
		middlew.CheckDb(middlew.ValidateJWT(GetContactHandler))).Methods("GET")
	router.HandleFunc("/api/contacts",
		middlew.CheckDb(middlew.ValidateJWT(AddContactHandler))).Methods("POST")
	router.HandleFunc("/api/contacts/{id}",
		middlew.CheckDb(middlew.ValidateJWT(EditContactHandler))).Methods("PUT")
	router.HandleFunc("/api/contacts/{id}",
		middlew.CheckDb(middlew.ValidateJWT(DeleteContactHandler))).Methods("DELETE")
	router.HandleFunc("/api/contacts/select2/q",
		middlew.CheckDb(middlew.ValidateJWT(GetContactWithSelect2Handler))).Methods("GET")
}

// GetContactsHandler endpoint cargar contactos.
func GetContactsHandler(w http.ResponseWriter, r *http.Request) {
	page := r.URL.Query().Get("page")
	search := r.URL.Query().Get("search")

	pagTemp, err := strconv.Atoi(page)
	if err != nil {
		http.Error(w, "Debe enviar el parámetro página como entero mayor a 0", http.StatusBadRequest)
		return
	}

	pag := int64(pagTemp)

	result, status := services.GetContacts(pag, search)
	if status == false {
		http.Error(w, "Error al leer los contactos", http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(result)
}

// GetContactHandler devuelve un contacto por id.
func GetContactHandler(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	contactId := vars["id"]
	if len(contactId) < 1 {
		http.Error(w, "Debe enviar el parámetro ID", http.StatusBadRequest)
		return
	}

	doc, err := services.GetContact(contactId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar buscar el registro, "+err.Error(), http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(doc)
}

// AddContactHandler endpoint registrar contacto.
func AddContactHandler(w http.ResponseWriter, r *http.Request) {
	var doc models.Contact
	err := json.NewDecoder(r.Body).Decode(&doc)
	if err != nil {
		http.Error(w, "Error en los datos recibidos "+err.Error(), http.StatusBadRequest)
		return
	}

	objID, status, err := services.AddContact(doc)
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

// EditContactHandler endpoint modificar contacto.
func EditContactHandler(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	contactId := vars["id"]

	var doc models.Contact

	err := json.NewDecoder(r.Body).Decode(&doc)
	if err != nil {
		http.Error(w, "Datos Incorrectos "+err.Error(), http.StatusBadRequest)
		return
	}

	status, err := services.UpdateContact(doc, contactId)
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

// DeleteContactHandler endpoint borrar contacto.
func DeleteContactHandler(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	contactId := vars["id"]
	if len(contactId) < 1 {
		http.Error(w, "Debe enviar el parámetro ID", http.StatusBadRequest)
		return
	}

	if !jwt.CheckDeletePermission() {
		http.Error(w, "Error no tiene permiso para eliminar este documento", http.StatusBadRequest)
		return
	}

	_, err := services.DeleteContact(contactId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar borrar, "+err.Error(), http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
}

// GetContactWithSelect2Handler buscar contactos con select2.
func GetContactWithSelect2Handler(w http.ResponseWriter, r *http.Request) {
	search := r.URL.Query().Get("term")

	var data models.Select2

	result, status := services.GetContacts(1, search)
	if status == false {
		http.Error(w, "Error al leer los contactos", http.StatusBadRequest)
		return
	}

	for i := 0; i < len(result); i++ {
		data.Results = append(data.Results, models.Select2Item{ID: result[i].ID.Hex(), Text: result[i].FullName})
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(data)
}
