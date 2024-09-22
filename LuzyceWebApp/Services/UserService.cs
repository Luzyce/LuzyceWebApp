using System.Net.Http.Json;
using Luzyce.Core.Models.Log;
using Luzyce.Core.Models.User;

namespace LuzyceWebApp.Services;

public class UserService(HttpClient httpClient, TokenValidationService tokenValidationService)
{
    public async Task<List<GetUserResponseDto>?> GetUsersAsync()
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return null;
        }
        return await httpClient.GetFromJsonAsync<List<GetUserResponseDto>>($"/api/user") ??
               [];
    }

    public async Task<GetUserForUpdateDto?> GetUserByIdAsync(int id)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return null;
        }
        return await httpClient.GetFromJsonAsync<GetUserForUpdateDto>($"api/user/{id}") ?? new GetUserForUpdateDto();
    }

    public async Task UpdateUserAsync(UpdateUserDto user, int id)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return;
        }
        await httpClient.PutAsJsonAsync($"api/user/{id}", user);
    }

    public async Task<List<GetRoleDto>?> GetRolesAsync()
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return null;
        }
        return await httpClient.GetFromJsonAsync<List<GetRoleDto>>("api/user/roles") ?? [];
    }

    public async Task ResetPasswordAsync(int userId, UpdatePasswordDto newPassword)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return;
        }
        await httpClient.PutAsJsonAsync($"api/user/{userId}/password", newPassword);
    }

    public async Task CreateUserAsync(CreateUserDto user)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return;
        }
        await httpClient.PostAsJsonAsync("/api/user", user);
    }

    public async Task<GetLogs> GetLogsAsync(int offset, int limit)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return new GetLogs();
        }
        return await httpClient.GetFromJsonAsync<GetLogs>($"api/log/unidentified/{offset}/{limit}") ?? new GetLogs();
    }

    public async Task AssignUserAsync(AssignUserDto assignUserDto)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return;
        }
        await httpClient.PutAsJsonAsync("api/log/assignUser", assignUserDto);
    }
}
