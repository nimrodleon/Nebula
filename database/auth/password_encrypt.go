package auth

import "golang.org/x/crypto/bcrypt"

// PasswordEncrypt rutina para encriptar contraseñas.
func PasswordEncrypt(passwordRaw string) (string, error) {
	bytes, err := bcrypt.GenerateFromPassword([]byte(passwordRaw), 8)
	return string(bytes), err
}
