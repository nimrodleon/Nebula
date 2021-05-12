package middlew

import (
	"net/http"
	"sgc-server/routes/jwt"
)

// ValidateJWT permite validar el JWT que nos viene en la petición.
func ValidateJWT(next http.HandlerFunc) http.HandlerFunc {
	return func(writer http.ResponseWriter, request *http.Request) {
		_, _, _, err := jwt.ProcessToken(request.Header.Get("Authorization"))
		if err != nil {
			http.Error(writer, "Error en el Token! "+err.Error(), http.StatusBadRequest)
			return
		}
		next.ServeHTTP(writer, request)
	}
}
