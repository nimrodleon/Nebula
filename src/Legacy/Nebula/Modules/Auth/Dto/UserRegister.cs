using System.ComponentModel.DataAnnotations;

namespace Nebula.Modules.Auth.Dto;

/// <summary>
/// Registrar Usuario.
/// </summary>
public class UserRegister
{
    /// <summary>
    /// Nombre de usuario.
    /// </summary>
    [Required(ErrorMessage = "El nombre de usuario es requerido.")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// E-Mail del usuario.
    /// </summary>
    [Required(ErrorMessage = "El correo electrónico es requerido.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña del usuario.
    /// </summary>
    [Required(ErrorMessage = "La contraseña es requerido.")]
    public string Password { get; set; } = string.Empty;
}
