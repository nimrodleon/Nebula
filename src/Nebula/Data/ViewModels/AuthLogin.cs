namespace Nebula.Data.ViewModels;

/// <summary>
/// Modelo para el inicio de session.
/// </summary>
public class AuthLogin
{
    /// <summary>
    /// Nombre de usuario.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Contraseña del usuario.
    /// </summary>
    public string Password { get; set; }
}
