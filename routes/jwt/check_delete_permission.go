package jwt

// CheckDeletePermission comprueba el permiso,
// para eliminar documentos de la base de datos.
func CheckDeletePermission() bool {
	if Permission == "ROLE_SUPER" || Permission == "ROLE_ADMIN" {
		return true
	}
	return false
}
