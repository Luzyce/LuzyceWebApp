using System.Net.Http.Json;
using Blazored.LocalStorage;
using Luzyce.Core.Models.User;

namespace LuzyceWebApp.Services;

public class UserService(HttpClient httpClient, ILocalStorageService localStorageService)
{
    public async Task<List<GetUserResponseDto>> GetUsersAsync()
    {
        var token = await localStorageService.GetItemAsStringAsync("token");
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new HttpRequestException("Token is missing");
        }

        httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        return await httpClient.GetFromJsonAsync<List<GetUserResponseDto>>("/api/user") ??
               new List<GetUserResponseDto>();
    }

    public async Task<GetUserForUpdateDto> GetUserByIdAsync(int id)
    {
        return await httpClient.GetFromJsonAsync<GetUserForUpdateDto>($"api/user/{id}") ?? new GetUserForUpdateDto();
    }

    public async Task UpdateUserAsync(UpdateUserDto user, int id)
    {
        await httpClient.PutAsJsonAsync($"api/user/{id}", user);
    }

    public async Task<List<GetRoleDto>> GetRolesAsync()
    {
        return await httpClient.GetFromJsonAsync<List<GetRoleDto>>("api/user/roles") ?? new List<GetRoleDto>();
    }

    public async Task ResetPasswordAsync(int userId, UpdatePasswordDto newPassword)
    {
        await httpClient.PutAsJsonAsync($"api/user/{userId}/password", newPassword);
    }

    public async Task CreateUserAsync(CreateUserDto user)
    {
        await httpClient.PostAsJsonAsync("/api/user", user);
    }
}