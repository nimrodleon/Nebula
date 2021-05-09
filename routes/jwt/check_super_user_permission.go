package jwt

// CheckSuperUserPermission permisos del super usuario.
func CheckSuperUserPermission() bool {
	return Permission == "ROLE_SUPER"
}
