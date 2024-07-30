## Gestión de Usuarios

Existen tres tipos de campos en el modelo de usuarios para configurar el sistema:

```csharp
public string AccountType { get; set; } = AccountTypeHelper.Personal; // Configura el tipo de cuenta
public string UserRole { get; set; } = UserRoleDbHelper.User;         // Configura el rol del usuario
public string DefaultCompanyId { get; set; } = string.Empty;          // Id por defecto de la empresa
```

### Restricciones de Uso

- El `AccountType` de tipo `business` se crea solo desde la vista pública.
- El tipo de cuenta `personal` solo puede ser creado por un usuario con rol `admin` de tipo `business`.
- Solo puede existir una cuenta de tipo `personal` por empresa.
- Solo una cuenta de tipo `business` puede crear empresas.
- Solo un usuario con rol `admin` puede borrar los registros.
- El usuario de tipo `business` siempre tendrá un rol `admin`.
- Solo existen dos roles en el sistema: `admin` y `user`.
