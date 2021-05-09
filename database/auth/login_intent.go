package auth

import (
	"golang.org/x/crypto/bcrypt"
	"sgt-server/models"
)

// LoginIntent realiza el chequeo de login a la BD.
func LoginIntent(userName string, password string) (models.User, bool) {
	user, found, _ := CheckUserExist(userName)
	if found == false {
		return user, false
	}

	passwordBytes := []byte(password)
	passwordDb := []byte(user.Password)
	err := bcrypt.CompareHashAndPassword(passwordDb, passwordBytes)
	if err != nil {
		return user, false
	}

	return user, true
}
