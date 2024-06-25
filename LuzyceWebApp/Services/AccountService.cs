using System.Net;
using Blazored.LocalStorage;
using Luzyce.Core.Models.User;
using LuzyceWebApp.Pages;

namespace LuzyceWebApp.Services;

public class AccountService(HttpClient httpClient, ILocalStorageService localStorageService)
    : IUserAccount
{
    private const string BaseUrl = "/api/login";
    private readonly ILocalStorageService localStorageService = localStorageService;

    public async Task<LoginResponseDto> LoginAccount(LoginDto loginDTO)
    {
        var response = await httpClient
           .PostAsync(BaseUrl,
                CustomAuthenticationStateProvider
                    .GenerateStringContent(CustomAuthenticationStateProvider.SerializeObj(loginDTO)));
        
        if (!response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return new LoginResponseDto()
            {
                Result = new GetUserResponseDto(),
                Token = ""
            };
        }
        
        var apiResponse = await response.Content.ReadAsStringAsync();
        return CustomAuthenticationStateProvider.DeserializeJsonString<LoginResponseDto>(apiResponse);

    }
}
