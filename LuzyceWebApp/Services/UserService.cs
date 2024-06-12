using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Blazored.LocalStorage;
using Luzyce.Core.Models.User;

namespace LuzyceWebApp.Services;

public class UserService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorageService;

    public UserService(HttpClient httpClient, ILocalStorageService localStorageService)
    {
        _httpClient = httpClient;
        _localStorageService = localStorageService;
    }

    public async Task<List<GetUserResponseDto>> GetUsersAsync()
    {
        string? token = await _localStorageService.GetItemAsStringAsync("token");
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new HttpRequestException("Token is missing");
        }

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        return await _httpClient.GetFromJsonAsync<List<GetUserResponseDto>>("/api/user") ?? [];
    }
}
