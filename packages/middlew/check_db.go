package middlew

import (
	"net/http"
	"sgc-server/packages/config"
)

// CheckDb middleware que permite conocer el estado de la BD.
func CheckDb(next http.HandlerFunc) http.HandlerFunc {
	return func(writer http.ResponseWriter, request *http.Request) {
		if config.CheckConnection() == 0 {
			http.Error(writer, "Conexión perdida con la base de datos", http.StatusInternalServerError)
			return
		}
		next.ServeHTTP(writer, request)
	}
}
