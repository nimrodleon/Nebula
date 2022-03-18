﻿namespace Nebula.Data.ViewModels;

/// <summary>
/// Registrar Usuario.
/// </summary>
public class UserRegister
{
    /// <summary>
    /// Nombre de usuario.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// E-Mail del usuario.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña del usuario.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Rol de usuario.
    /// </summary>
    public string Role { get; set; } = string.Empty;
}
