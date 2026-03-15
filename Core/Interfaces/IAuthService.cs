using GlamoraApi.DTOs;


namespace GlamoraApi.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterDto dto);
        Task<AuthResult>LoginAsync(LoginDto dto);
    }
}

