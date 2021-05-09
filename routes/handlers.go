package routes

import (
	"github.com/gorilla/mux"
	"github.com/rs/cors"
	"log"
	"net/http"
	"sgt-server/routes/articleHandler"
	"sgt-server/routes/authHandler"
	"sgt-server/routes/contactHandler"
	"sgt-server/routes/deviceTypeHandler"
	"sgt-server/routes/middlew"
	"sgt-server/routes/taxesHandler"
	"sgt-server/routes/userHandler"
	"sgt-server/routes/warehouseHandler"
)

func Handlers() {
	router := mux.NewRouter()

	// User & Auth Handlers.
	router.HandleFunc("/api/auth/login", middlew.CheckDb(authHandler.Login)).Methods("POST")
	router.HandleFunc("/api/auth/register_super_user", middlew.CheckDb(authHandler.RegisterSuperUser)).Methods("POST")
	router.HandleFunc("/api/users", middlew.CheckDb(middlew.ValidateJWT(userHandler.LoadUsers))).Methods("GET")
	router.HandleFunc("/api/users/{id}", middlew.CheckDb(middlew.ValidateJWT(userHandler.GetUser))).Methods("GET")
	router.HandleFunc("/api/users", middlew.CheckDb(middlew.ValidateJWT(userHandler.AddUser))).Methods("POST")
	router.HandleFunc("/api/users/{id}", middlew.CheckDb(middlew.ValidateJWT(userHandler.EditUser))).Methods("PUT")
	router.HandleFunc("/api/users/{id}", middlew.CheckDb(middlew.ValidateJWT(userHandler.DeleteUser))).Methods("DELETE")
	router.HandleFunc("/api/users/change_status/{id}/{status}", middlew.CheckDb(middlew.ValidateJWT(userHandler.ChangeStatus))).Methods("PATCH")
	router.HandleFunc("/api/users/password_change/{id}", middlew.CheckDb(middlew.ValidateJWT(userHandler.PasswordChange))).Methods("PATCH")
	router.HandleFunc("/api/users/select2/q", middlew.CheckDb(middlew.ValidateJWT(userHandler.FindUsersWithSelect2))).Methods("GET")

	// Contact Handlers.
	router.HandleFunc("/api/contacts", middlew.CheckDb(middlew.ValidateJWT(contactHandler.LoadContacts))).Methods("GET")
	router.HandleFunc("/api/contacts/{id}", middlew.CheckDb(middlew.ValidateJWT(contactHandler.GetContact))).Methods("GET")
	router.HandleFunc("/api/contacts", middlew.CheckDb(middlew.ValidateJWT(contactHandler.AddContact))).Methods("POST")
	router.HandleFunc("/api/contacts/{id}", middlew.CheckDb(middlew.ValidateJWT(contactHandler.EditContact))).Methods("PUT")
	router.HandleFunc("/api/contacts/{id}", middlew.CheckDb(middlew.ValidateJWT(contactHandler.DeleteContact))).Methods("DELETE")
	router.HandleFunc("/api/contacts/select2/q", middlew.CheckDb(middlew.ValidateJWT(contactHandler.FindContactWithSelect2))).Methods("GET")

	// Taxes Handlers.
	router.HandleFunc("/api/taxes", middlew.CheckDb(middlew.ValidateJWT(taxesHandler.LoadTaxes))).Methods("GET")
	router.HandleFunc("/api/taxes/{id}", middlew.CheckDb(middlew.ValidateJWT(taxesHandler.GetTax))).Methods("GET")
	router.HandleFunc("/api/taxes", middlew.CheckDb(middlew.ValidateJWT(taxesHandler.AddTax))).Methods("POST")
	router.HandleFunc("/api/taxes/{id}", middlew.CheckDb(middlew.ValidateJWT(taxesHandler.EditTax))).Methods("PUT")
	router.HandleFunc("/api/taxes/{id}", middlew.CheckDb(middlew.ValidateJWT(taxesHandler.DeleteTax))).Methods("DELETE")

	// Articles Handlers.
	router.HandleFunc("/api/articles", middlew.CheckDb(middlew.ValidateJWT(articleHandler.LoadArticles))).Methods("GET")
	router.HandleFunc("/api/articles/{id}", middlew.CheckDb(middlew.ValidateJWT(articleHandler.GetArticle))).Methods("GET")
	router.HandleFunc("/api/articles", middlew.CheckDb(middlew.ValidateJWT(articleHandler.AddArticle))).Methods("POST")
	router.HandleFunc("/api/articles/{id}", middlew.CheckDb(middlew.ValidateJWT(articleHandler.EditArticle))).Methods("PUT")
	router.HandleFunc("/api/articles/{id}", middlew.CheckDb(middlew.ValidateJWT(articleHandler.DeleteArticle))).Methods("DELETE")

	// Warehouses Handlers.
	router.HandleFunc("/api/warehouses", middlew.CheckDb(middlew.ValidateJWT(warehouseHandler.LoadWarehouses))).Methods("GET")
	router.HandleFunc("/api/warehouses/{id}", middlew.CheckDb(middlew.ValidateJWT(warehouseHandler.GetWarehouse))).Methods("GET")
	router.HandleFunc("/api/warehouses", middlew.CheckDb(middlew.ValidateJWT(warehouseHandler.AddWarehouse))).Methods("POST")
	router.HandleFunc("/api/warehouses/{id}", middlew.CheckDb(middlew.ValidateJWT(warehouseHandler.EditWarehouse))).Methods("PUT")
	router.HandleFunc("/api/warehouses/{id}", middlew.CheckDb(middlew.ValidateJWT(warehouseHandler.DeleteWarehouse))).Methods("DELETE")
	router.HandleFunc("/api/warehouses/{type}/select2/q", middlew.CheckDb(middlew.ValidateJWT(warehouseHandler.FindWarehousesWithSelect2))).Methods("GET")

	// DeviceTypes Handlers.
	router.HandleFunc("/api/device_types", middlew.CheckDb(middlew.ValidateJWT(deviceTypeHandler.LoadDeviceType))).Methods("GET")
	router.HandleFunc("/api/device_types/{id}", middlew.CheckDb(middlew.ValidateJWT(deviceTypeHandler.GetDeviceType))).Methods("GET")
	router.HandleFunc("/api/device_types", middlew.CheckDb(middlew.ValidateJWT(deviceTypeHandler.AddDeviceType))).Methods("POST")
	router.HandleFunc("/api/device_types/{id}", middlew.CheckDb(middlew.ValidateJWT(deviceTypeHandler.EditDeviceType))).Methods("PUT")
	router.HandleFunc("/api/device_types/{id}", middlew.CheckDb(middlew.ValidateJWT(deviceTypeHandler.DeleteDeviceType))).Methods("DELETE")
	router.HandleFunc("/api/device_types/select2/q", middlew.CheckDb(middlew.ValidateJWT(deviceTypeHandler.FindDeviceTypeWithSelect2))).Methods("GET")

	// Run HTTP Server.
	handler := cors.AllowAll().Handler(router)
	log.Fatal(http.ListenAndServe(":8084", handler))
}
