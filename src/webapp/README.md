# Facturador

Software de Facturación Electrónica.

## Estructura de los Libros y Registros Electrónicos

- Estructuras del PLE

```bash
https://emprender.sunat.gob.pe/estructurasple
```

- [Registro de Compras I](https://youtu.be/jxucTvSz4Hs)
- [Registro de Compras II](https://youtu.be/W5usHJmaNbQ)

## Estructura gestión de usuarios

existen 3 tipos de campos en el modelo de usuarios para configurar el sistema

```C#
  public string AccountType { get; set; } = AccountTypeHelper.Personal; // configura el tipo de cuenta
  public string UserRole { get; set; } = UserRoleDbHelper.User;         // configura el rol del usuario
  public string DefaultCompanyId { get; set; } = string.Empty;          // Id por defecto de la empresa
```

### Restricciones de uso

- el AccountType **bussiness** se crea solo desde la vista pública.
- el tipo de cuenta **personal** solo puede crear un usuario con rol **admin** de tipo **business**.
- solo puede existir una cuenta de tipo **personal** por empresa.
- solo una cuenta de tipo **business** puede crear empresas.
- solo un usuario con rol **admin** puede borrar los registros.
- el usuario de tipo **business** siempre tendra un rol **admin**.
- solo existen dos roles en el sistema **admin|user**.
