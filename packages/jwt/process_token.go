package jwt

import (
	"errors"
	"github.com/dgrijalva/jwt-go"
	"log"
	"sgc-server/packages/models"
	"sgc-server/packages/services"
	"strings"
)

// UserId es Id devuelto del modelo, que usará en todos los endpoints.
var UserId string

// UserName usado en todos los endpoints.
var UserName string

// Permission es el nivel de acceso que usará en todos los endpoints.
var Permission string

// ProcessToken proceso token para extraer sus valores.
func ProcessToken(tk string) (*models.Claim, bool, string, error) {
	myKey := []byte("super_secret")
	claims := &models.Claim{}

	splitToken := strings.Split(tk, "Bearer")
	if len(splitToken) != 2 {
		return claims, false, string(""), errors.New("formato de token invalido")
	}

	tk = strings.TrimSpace(splitToken[1])
	tkn, err := jwt.ParseWithClaims(tk, claims, func(token *jwt.Token) (interface{}, error) {
		return myKey, nil
	})
	if err == nil {
		_, found, _ := services.CheckUserExist(claims.UserName)
		if found == true {
			UserName = claims.UserName
			Permission = claims.Permission
			UserId = claims.ID.Hex()
		}
		// Imprimir por consola.
		log.Println(UserId, UserName, Permission)
		return claims, found, UserId, nil
	}
	if !tkn.Valid {
		return claims, false, string(""), errors.New("token Inválido")
	}
	return claims, false, string(""), err
}
