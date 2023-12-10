namespace Nebula.Modules.Auth.Dto;

/// <summary>
/// Modelo para el inicio de session.
/// </summary>
public class AuthLogin
{
    /// <summary>
    /// E-Mail del usuario.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña del usuario.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
