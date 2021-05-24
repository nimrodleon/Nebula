package controllers

import (
	"encoding/json"
	"fmt"
	"github.com/gorilla/mux"
	"net/http"
	"sgc-server/packages/jwt"
	"sgc-server/packages/middlew"
	"sgc-server/packages/models"
	"sgc-server/packages/repository"
	"strconv"
)

func DeviceTypeRouterHandler(router *mux.Router) {
	router.HandleFunc("/api/device_types", middlew.CheckDb(middlew.ValidateJWT(GetDeviceTypesHandler))).Methods("GET")
	router.HandleFunc("/api/device_types/{id}", middlew.CheckDb(middlew.ValidateJWT(GetDeviceTypeHandler))).Methods("GET")
	router.HandleFunc("/api/device_types", middlew.CheckDb(middlew.ValidateJWT(AddDeviceTypeHandler))).Methods("POST")
	router.HandleFunc("/api/device_types/{id}", middlew.CheckDb(middlew.ValidateJWT(EditDeviceTypeHandler))).Methods("PUT")
	router.HandleFunc("/api/device_types/{id}", middlew.CheckDb(middlew.ValidateJWT(DeleteDeviceTypeHandler))).Methods("DELETE")
	router.HandleFunc("/api/device_types/select2/q", middlew.CheckDb(middlew.ValidateJWT(GetDeviceTypeWithSelect2Handler))).Methods("GET")
}

// GetDeviceTypesHandler cargar tipos de equipo.
func GetDeviceTypesHandler(w http.ResponseWriter, r *http.Request) {
	page := r.URL.Query().Get("page")
	search := r.URL.Query().Get("search")

	pagTemp, err := strconv.Atoi(page)
	if err != nil {
		http.Error(w, "Debe enviar el parámetro página como entero mayor a 0", http.StatusBadRequest)
		return
	}

	pag := int64(pagTemp)

	result, status := repository.GetDeviceTypes(pag, search)
	if status == false {
		http.Error(w, "Error al leer los impuestos", http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(result)
}

// GetDeviceTypeHandler retorna un tipo de equipo.
func GetDeviceTypeHandler(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	deviceTypeId := vars["id"]
	if len(deviceTypeId) < 1 {
		http.Error(w, "Debe enviar el parámetro ID", http.StatusBadRequest)
		return
	}

	doc, err := repository.GetDeviceType(deviceTypeId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar buscar el registro, "+err.Error(), http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(doc)
}

// AddDeviceTypeHandler agregar el tipo de equipo.
func AddDeviceTypeHandler(w http.ResponseWriter, r *http.Request) {
	var doc models.DeviceType
	err := json.NewDecoder(r.Body).Decode(&doc)
	if err != nil {
		http.Error(w, "Error en los datos recibidos "+err.Error(), http.StatusBadRequest)
		return
	}

	objID, status, err := repository.AddDeviceType(doc)
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

// EditDeviceTypeHandler editar tipo de equipo.
func EditDeviceTypeHandler(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	deviceTypeId := vars["id"]

	var doc models.DeviceType

	err := json.NewDecoder(r.Body).Decode(&doc)
	if err != nil {
		http.Error(w, "Datos Incorrectos "+err.Error(), http.StatusBadRequest)
		return
	}

	status, err := repository.UpdateDeviceType(doc, deviceTypeId)
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

// DeleteDeviceTypeHandler borra un tipo de equipo.
func DeleteDeviceTypeHandler(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	deviceTypeId := vars["id"]
	if len(deviceTypeId) < 1 {
		http.Error(w, "Debe enviar el parámetro ID", http.StatusBadRequest)
		return
	}

	if !jwt.CheckDeletePermission() {
		http.Error(w, "Error no tiene permiso para eliminar este documento", http.StatusBadRequest)
		return
	}

	_, err := repository.DeleteDeviceType(deviceTypeId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar borrar, "+err.Error(), http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
}

// GetDeviceTypeWithSelect2Handler buscar tipos de equipo con select2.
func GetDeviceTypeWithSelect2Handler(w http.ResponseWriter, r *http.Request) {
	search := r.URL.Query().Get("term")

	var data models.Select2

	result, status := repository.GetDeviceTypes(1, search)
	if status == false {
		http.Error(w, "Error al leer los contactos", http.StatusBadRequest)
		return
	}

	for i := 0; i < len(result); i++ {
		data.Results = append(data.Results, models.Select2Item{ID: result[i].ID.Hex(), Text: result[i].Name})
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(data)
}
