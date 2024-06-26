using System.Net;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using Luzyce.Core.Models.Order;
using Luzyce.Core.Models.ProductionOrder;

namespace LuzyceWebApp.Services;

public class OrderService(HttpClient httpClient, ILocalStorageService localStorageService)
{
    public async Task<GetOrdersResponseDto> GetOrdersAsync(int pageNumber, GetOrdersDto getOrdersDto,
        CancellationToken cancellationToken)
    {
        var token = await localStorageService.GetItemAsStringAsync("token", cancellationToken);
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new HttpRequestException("Token is missing");
        }

        httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await httpClient.PostAsJsonAsync($"/api/order/{pageNumber}", getOrdersDto, cancellationToken);
        return await response.Content.ReadFromJsonAsync<GetOrdersResponseDto>(cancellationToken) ??
               new GetOrdersResponseDto();
    }
    
    public async Task<bool> CreateOrderAsync(CreateProductionOrderDto createProductionOrderDto)
    {
        var response = await httpClient.PostAsJsonAsync("/api/productionOrder/new", createProductionOrderDto);
        return response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.Unauthorized && response.StatusCode != HttpStatusCode.Conflict;
    }
}