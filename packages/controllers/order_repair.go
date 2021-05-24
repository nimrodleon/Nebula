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

func OrderRepairRouterHandler(router *mux.Router) {
	router.HandleFunc("/api/order_repairs", middlew.CheckDb(middlew.ValidateJWT(GetOrderRepairsHandler))).Methods("GET")
	router.HandleFunc("/api/order_repairs/{id}", middlew.CheckDb(middlew.ValidateJWT(GetOrderRepairHandler))).Methods("GET")
	router.HandleFunc("/api/order_repairs", middlew.CheckDb(middlew.ValidateJWT(AddOrderRepairHandler))).Methods("POST")
	router.HandleFunc("/api/order_repairs/{id}", middlew.CheckDb(middlew.ValidateJWT(EditOrderRepairHandler))).Methods("PUT")
	router.HandleFunc("/api/order_repairs/{id}", middlew.CheckDb(middlew.ValidateJWT(DeleteOrderRepairHandler))).Methods("DELETE")
}

// GetOrderRepairsHandler carga las ordenes de reparación.
func GetOrderRepairsHandler(w http.ResponseWriter, r *http.Request) {
	page := r.URL.Query().Get("page")
	search := r.URL.Query().Get("search")

	pagTemp, err := strconv.Atoi(page)
	if err != nil {
		http.Error(w, "Debe enviar el parámetro página como entero mayor a 0", http.StatusBadRequest)
		return
	}

	pag := int64(pagTemp)

	result, status := repository.GetOrderRepairs(pag, search)
	if status == false {
		http.Error(w, "Error al leer las ordenes de reparación", http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(result)
}

// GetOrderRepairHandler obtiene una orden de reparación filtrado por id.
func GetOrderRepairHandler(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	orderRepairId := vars["id"]
	if len(orderRepairId) < 1 {
		http.Error(w, "Debe enviar el parámetro ID", http.StatusBadRequest)
		return
	}

	doc, err := repository.GetOrderRepair(orderRepairId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar buscar el registro, "+err.Error(), http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
	_ = json.NewEncoder(w).Encode(doc)
}

// AddOrderRepairHandler agrega una orden de reparación.
func AddOrderRepairHandler(w http.ResponseWriter, r *http.Request) {
	var doc models.OrderRepair
	err := json.NewDecoder(r.Body).Decode(&doc)
	if err != nil {
		http.Error(w, "Error en los datos recibidos "+err.Error(), http.StatusBadRequest)
		return
	}

	objID, status, err := repository.AddOrderRepair(doc)
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

// EditOrderRepairHandler actualiza la información de una orden de reparación.
func EditOrderRepairHandler(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	orderRepairId := vars["id"]

	var doc models.OrderRepair

	err := json.NewDecoder(r.Body).Decode(&doc)
	if err != nil {
		http.Error(w, "Datos Incorrectos "+err.Error(), http.StatusBadRequest)
		return
	}

	status, err := repository.UpdateOrderRepair(doc, orderRepairId)
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

// DeleteOrderRepairHandler borra una orden de reparación.
func DeleteOrderRepairHandler(w http.ResponseWriter, r *http.Request) {
	vars := mux.Vars(r)
	orderRepairId := vars["id"]
	if len(orderRepairId) < 1 {
		http.Error(w, "Debe enviar el parámetro ID", http.StatusBadRequest)
		return
	}

	if !jwt.CheckDeletePermission() {
		http.Error(w, "Error no tiene permiso para eliminar este documento", http.StatusBadRequest)
		return
	}

	_, err := repository.DeleteOrderRepair(orderRepairId)
	if err != nil {
		http.Error(w, "Ocurrió un error al intentar borrar, "+err.Error(), http.StatusBadRequest)
		return
	}

	w.Header().Set("Content-Type", "application/json")
	w.WriteHeader(http.StatusCreated)
}
