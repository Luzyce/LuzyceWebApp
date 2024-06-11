using Luzyce.Core.Models.User;

namespace LuzyceWebApp.Services;

public interface IUserAccount
{
    Task<LoginResponseDto> LoginAccount(LoginDto loginDTO);
}