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

func WarehouseRouterHandler(router *mux.Router) {
	router.HandleFunc("/api/warehouses",
		middlew.CheckDb(middlew.ValidateJWT(GetWarehousesHandler))).Methods("GET")
	router.HandleFunc("/api/warehouses/{id}",
		middlew.CheckDb(middlew.ValidateJWT(GetWarehouseHandler))).Methods("GET")
	router.HandleFunc("/api/warehouses",
		middlew.CheckDb(middlew.ValidateJWT(AddWarehouseHandler))).Methods("POST")
	router.HandleFunc("/api/warehouses/{id}",
		middlew.CheckDb(middlew.ValidateJWT(EditWarehouseHandler))).Methods("PUT")
	router.HandleFunc("/api/warehouses/{id}",
		middlew.CheckDb(middlew.ValidateJWT(DeleteWarehouseHandler))).Methods("DELETE")
	router.HandleFunc("/api/warehouses/{type}/select2/q",
		middlew.CheckDb(middlew.ValidateJWT(GetWarehousesWithSelect2Handler))).Methods("GET")
}

// GetWarehousesHandler cargar almacenes.
func GetWarehousesHandler(w http.ResponseWriter, r *http.Request) {
	page := r.URL.Query().Get("page")
	search := r.URL.Query().Get("search")

	pagTemp, err := strconv.Atoi(page)
	if err != nil {
		http.Error(w, "Debe enviar el parámetro página como entero mayor a 0", http.StatusBadRequest)
		return
	}

	pag := int64(pagTemp)

	result, status := services.GetWarehouses(pag, search)
	if status == false {
		http.Error(w, "Error al leer los almacenes", http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(result)
}

// GetWarehouseHandler retorna un almacén.
func GetWarehouseHandler(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	warehouseId := vars["id"]
	if len(warehouseId) < 1 {
		http.Error(w, "Debe enviar el parámetro ID", http.StatusBadRequest)
		return
	}

	doc, err := services.GetWarehouse(warehouseId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar buscar el registro, "+err.Error(), http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(doc)
}

// AddWarehouseHandler agrega un almacén.
func AddWarehouseHandler(w http.ResponseWriter, r *http.Request) {
	var doc models.Warehouse
	err := json.NewDecoder(r.Body).Decode(&doc)
	if err != nil {
		http.Error(w, "Error en los datos recibidos "+err.Error(), http.StatusBadRequest)
		return
	}

	objID, status, err := services.AddWarehouse(doc)
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

// EditWarehouseHandler editar almacén.
func EditWarehouseHandler(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	warehouseId := vars["id"]

	var doc models.Warehouse

	err := json.NewDecoder(r.Body).Decode(&doc)
	if err != nil {
		http.Error(w, "Datos Incorrectos "+err.Error(), http.StatusBadRequest)
		return
	}

	status, err := services.UpdateWarehouse(doc, warehouseId)
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

// DeleteWarehouseHandler borrar almacenes.
func DeleteWarehouseHandler(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	warehouseId := vars["id"]
	if len(warehouseId) < 1 {
		http.Error(w, "Debe enviar el parámetro ID", http.StatusBadRequest)
		return
	}

	if !jwt.CheckDeletePermission() {
		http.Error(w, "Error no tiene permiso para eliminar este documento", http.StatusBadRequest)
		return
	}

	_, err := services.DeleteWarehouse(warehouseId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar borrar, "+err.Error(), http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
}

// GetWarehousesWithSelect2Handler buscar almacenes desde select2.
func GetWarehousesWithSelect2Handler(w http.ResponseWriter, r *http.Request) {
	search := r.URL.Query().Get("term")
	vars := mux.Vars(r)
	typeWarehouse := vars["type"]

	if len(typeWarehouse) < 1 {
		http.Error(w, "Debe enviar el parámetro ID", http.StatusBadRequest)
		return
	}

	var data models.Select2

	result, status := services.GetWarehousesWithType(typeWarehouse, search)
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
