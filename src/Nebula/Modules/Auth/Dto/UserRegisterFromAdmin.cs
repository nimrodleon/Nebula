using Nebula.Modules.Auth.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Nebula.Modules.Auth.Dto;

public class UserRegisterFromAdmin
{
    [Required(ErrorMessage = "El nombre de usuario es requerido.")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es requerido.")]
    public string Email { get; set; } = string.Empty;

    // [Required(ErrorMessage = "El tipo de usuario es requerido.")]
    // public string UserType { get; set; } = UserTypeSystem.Customer;

    [Required(ErrorMessage = "El estado del usuario es requerido.")]
    public bool Disabled { get; set; } = false;
}
