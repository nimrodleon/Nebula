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

func TaxRouterHandler(router *mux.Router) {
	router.HandleFunc("/api/taxes",
		middlew.CheckDb(middlew.ValidateJWT(GetTaxesHandler))).Methods("GET")
	router.HandleFunc("/api/taxes/{id}",
		middlew.CheckDb(middlew.ValidateJWT(GetTaxHandler))).Methods("GET")
	router.HandleFunc("/api/taxes",
		middlew.CheckDb(middlew.ValidateJWT(AddTaxHandler))).Methods("POST")
	router.HandleFunc("/api/taxes/{id}",
		middlew.CheckDb(middlew.ValidateJWT(EditTaxHandler))).Methods("PUT")
	router.HandleFunc("/api/taxes/{id}",
		middlew.CheckDb(middlew.ValidateJWT(DeleteTaxHandler))).Methods("DELETE")
}

// GetTaxesHandler cargar los impuestos.
func GetTaxesHandler(w http.ResponseWriter, r *http.Request) {
	page := r.URL.Query().Get("page")
	search := r.URL.Query().Get("search")

	pagTemp, err := strconv.Atoi(page)
	if err != nil {
		http.Error(w, "Debe enviar el parámetro página como entero mayor a 0", http.StatusBadRequest)
		return
	}

	pag := int64(pagTemp)

	result, status := services.GetTaxes(pag, search)
	if status == false {
		http.Error(w, "Error al leer los impuestos", http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(result)
}

// GetTaxHandler retorna un impuesto en especifico.
func GetTaxHandler(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	taxId := vars["id"]
	if len(taxId) < 1 {
		http.Error(w, "Debe enviar el parámetro ID", http.StatusBadRequest)
		return
	}

	doc, err := services.GetTax(taxId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar buscar el registro, "+err.Error(), http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(doc)
}

// AddTaxHandler agrega un registro a la DB.
func AddTaxHandler(w http.ResponseWriter, r *http.Request) {
	var doc models.Tax
	err := json.NewDecoder(r.Body).Decode(&doc)
	if err != nil {
		http.Error(w, "Error en los datos recibidos "+err.Error(), http.StatusBadRequest)
		return
	}

	objID, status, err := services.AddTax(doc)
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

// EditTaxHandler editar impuesto.
func EditTaxHandler(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	taxId := vars["id"]

	var doc models.Tax

	err := json.NewDecoder(r.Body).Decode(&doc)
	if err != nil {
		http.Error(w, "Datos Incorrectos "+err.Error(), http.StatusBadRequest)
		return
	}

	status, err := services.UpdateTax(doc, taxId)
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

// DeleteTaxHandler borra un impuesto.
func DeleteTaxHandler(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	taxId := vars["id"]
	if len(taxId) < 1 {
		http.Error(w, "Debe enviar el parámetro ID", http.StatusBadRequest)
		return
	}

	if !jwt.CheckDeletePermission() {
		http.Error(w, "Error no tiene permiso para eliminar este documento", http.StatusBadRequest)
		return
	}

	_, err := services.DeleteTax(taxId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar borrar, "+err.Error(), http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
}
