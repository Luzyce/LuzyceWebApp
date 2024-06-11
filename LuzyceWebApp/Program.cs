using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using LuzyceWebApp;
using LuzyceWebApp.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using LuzyceWebApp.Pages;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://phsapp.getin.ovh/") });
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IUserAccount, AccountService>();
builder.Services.AddScoped<AccountService>();

await builder.Build().RunAsync();