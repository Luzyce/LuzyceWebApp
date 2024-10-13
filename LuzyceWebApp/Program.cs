using Append.Blazor.Printing;
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

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.Configuration["apiUrl"]!) });
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<IPrintingService, PrintingService>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<TokenValidationService>();
builder.Services.AddScoped<ProductionOrderService>();
builder.Services.AddScoped<ProdPrioritiesService>();
builder.Services.AddScoped<TokenRefreshService>();
builder.Services.AddScoped<ProductionPlanService>();
builder.Services.AddScoped<DocumentDependencyChartService>();

await builder.Build().RunAsync();