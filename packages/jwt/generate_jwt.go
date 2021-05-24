package jwt

import (
	"github.com/dgrijalva/jwt-go"
	"sgc-server/packages/models"
	"time"
)

// GenerateJWT genera el encriptado con JWT.
func GenerateJWT(t models.User) (string, error) {
	myKey := []byte("super_secret")
	payload := jwt.MapClaims{
		"_id":        t.ID.Hex(),
		"full_name":  t.FullName,
		"user_name":  t.UserName,
		"permission": t.Permission,
		"email":      t.Email,
		"exp":        time.Now().Add(time.Hour * 24).Unix(),
	}
	token := jwt.NewWithClaims(jwt.SigningMethodHS256, payload)
	tokenStr, err := token.SignedString(myKey)
	if err != nil {
		return tokenStr, err
	}
	return tokenStr, nil
}
