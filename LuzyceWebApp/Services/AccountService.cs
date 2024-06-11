using Blazored.LocalStorage;
using Luzyce.Core.Models.User;
using LuzyceWebApp.Pages;
using static LuzyceWebApp.Pages.Weather;

namespace LuzyceWebApp.Services;

public class AccountService : IUserAccount
{
    public AccountService(HttpClient httpClient, ILocalStorageService localStorageService)
    {
        this.httpClient = httpClient;
        this.localStorageService = localStorageService;
    }
    private const string BaseUrl = "/api/login";
    private readonly HttpClient httpClient;
    private readonly ILocalStorageService localStorageService;

    public async Task<LoginResponseDto> LoginAccount(LoginDto loginDTO)
    {
        var response = await httpClient
           .PostAsync(BaseUrl,
                CustomAuthenticationStateProvider
                    .GenerateStringContent(CustomAuthenticationStateProvider.SerializeObj(loginDTO)));

        //Read Response
        if (!response.IsSuccessStatusCode)
            return new LoginResponseDto()
            {
                Result = null!,
                Token = null!
            };

        string apiResponse = await response.Content.ReadAsStringAsync();
        return CustomAuthenticationStateProvider.DeserializeJsonString<LoginResponseDto>(apiResponse);

    }
}
