namespace Nebula.Modules.Auth.Dto;

public class ResetPasswordRequest
{
    public string Token { get; set; }
    public string NewPassword { get; set; } = string.Empty;
}
