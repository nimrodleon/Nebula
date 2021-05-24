package main

import (
	"github.com/gorilla/mux"
	"github.com/rs/cors"
	"log"
	"net/http"
	"sgc-server/packages/config"
	"sgc-server/packages/controllers"
)

func ServeHTTP() {
	router := mux.NewRouter()
	controllers.UserRouterHandler(router)
	controllers.ArticleRouterHandler(router)
	controllers.TaxRouterHandler(router)
	controllers.WarehouseRouterHandler(router)
	controllers.ContactRouterHandler(router)
	controllers.DeviceTypeRouterHandler(router)
	controllers.OrderRepairRouterHandler(router)
	handler := cors.AllowAll().Handler(router)
	log.Fatal(http.ListenAndServe(":8084", handler))
}

func main() {
	if config.CheckConnection() == 0 {
		log.Fatal("Sin conexión a la BD")
		return
	}
	ServeHTTP()
}
