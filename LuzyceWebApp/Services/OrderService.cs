using System.Net;
using System.Net.Http.Json;
using Luzyce.Core.Models.Lampshade;
using Luzyce.Core.Models.Order;
using Luzyce.Core.Models.ProductionOrder;

namespace LuzyceWebApp.Services;

public class OrderService(HttpClient httpClient, TokenValidationService tokenValidationService)
{
    public async Task<GetOrdersResponseDto?> GetOrdersAsync(int pageNumber, GetOrdersDto getOrdersDto,
        CancellationToken cancellationToken)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return null;
        }
        var response = await httpClient.PostAsJsonAsync($"/api/order/{pageNumber}", getOrdersDto, cancellationToken);
        return await response.Content.ReadFromJsonAsync<GetOrdersResponseDto>(cancellationToken) ??
               new GetOrdersResponseDto();
    }
    
    public async Task<bool> CreateOrderAsync(CreateProductionOrderDto createProductionOrderDto)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return false;
        }
        var response = await httpClient.PostAsJsonAsync("/api/productionOrder/new", createProductionOrderDto);
        return response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.Unauthorized && response.StatusCode != HttpStatusCode.Conflict;
    }
    
    public async Task<GetVariantsResponseDto?> GetVariantsAsync()
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return new GetVariantsResponseDto();
        }
        var response = await httpClient.GetAsync("api/lampshade/variants");
        return await response.Content.ReadFromJsonAsync<GetVariantsResponseDto>();
    }
}