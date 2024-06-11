using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;
using LuzyceWebApp.AppModels;
using System.IdentityModel.Tokens.Jwt;

namespace LuzyceWebApp.Services;

public class CustomAuthenticationStateProvider(ILocalStorageService localStorageService) : AuthenticationStateProvider
{
    private ClaimsPrincipal anonymous = new(new ClaimsIdentity());
    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            string stringToken = await localStorageService.GetItemAsStringAsync("token");

            if (string.IsNullOrWhiteSpace(stringToken))
                return await Task.FromResult(new AuthenticationState(anonymous));

            var claims = GetClaimsFromToken(stringToken);

            var claimsPrincipal = SetClaimPrincipal(claims);
            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }
        catch
        {
            return await Task.FromResult(new AuthenticationState(anonymous));
        }
    }

    public async Task UpdateAuthenticationState(string? token)
    {
        ClaimsPrincipal claimsPrincipal = new();
        if (!string.IsNullOrWhiteSpace(token))
        {
            var userSession = GetClaimsFromToken(token);
            claimsPrincipal = SetClaimPrincipal(userSession);
            await localStorageService.SetItemAsStringAsync("token", token);
        }
        else
        {
            claimsPrincipal = anonymous;
            await localStorageService.RemoveItemAsync("token");
        }
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }


    public static ClaimsPrincipal SetClaimPrincipal(UserSession model)
    {
        return new ClaimsPrincipal(new ClaimsIdentity(
            new List<Claim>
            {
                    new(ClaimTypes.NameIdentifier, model.Id!),
                    new(ClaimTypes.Email, model.Email!),
                    new(ClaimTypes.Role, model.Role!),
                    new(ClaimTypes.GivenName, model.Name!),
            }, "JwtAuth"));
    }

    public static UserSession GetClaimsFromToken(string jwtToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwtToken);
        var claims = token.Claims;

        string Id = claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value!;
        string Email = claims.First(c => c.Type == ClaimTypes.Email).Value!;
        string Role = claims.First(c => c.Type == ClaimTypes.Role).Value!;
        string Name = claims.First(c => c.Type == ClaimTypes.GivenName).Value!;

        return new UserSession() { Id = Id, Name = Name, Email = Email, Role = Role };
    }

    public static JsonSerializerOptions JsonOptions()
    {
        return new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
        };
    }
    public static string SerializeObj<T>(T modelObject) => JsonSerializer.Serialize(modelObject, JsonOptions());
    public static T DeserializeJsonString<T>(string jsonString) => JsonSerializer.Deserialize<T>(jsonString, JsonOptions())!;
    public static IList<T> DeserializeJsonStringList<T>(string jsonString) => JsonSerializer.Deserialize<IList<T>>(jsonString, JsonOptions())!;

    public static StringContent GenerateStringContent(string serialiazedObj) => new(serialiazedObj, System.Text.Encoding.UTF8, "application/json");

}
