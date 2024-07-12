using System.Net;
using System.Net.Http.Json;
using Luzyce.Core.Models.ProductionOrder;

namespace LuzyceWebApp.Services;

public class ProductionOrderService(HttpClient httpClient, TokenValidationService tokenValidationService)
{
    public async Task<GetProductionOrdersResponse?> GetProductionOrders()
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return null;
        }
        return await httpClient.GetFromJsonAsync<GetProductionOrdersResponse>($"/api/productionOrder");
    }
    
    public async Task<GetProductionOrder?> GetProductionOrder(int id)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return null;
        }
        return await httpClient.GetFromJsonAsync<GetProductionOrder>($"/api/productionOrder/{id}");
    }
    
    public async Task<bool> CreateProductionOrderAsync(CreateProductionOrderRequest createProductionOrderDto)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return false;
        }
        var response = await httpClient.PostAsJsonAsync("/api/productionOrder/new", createProductionOrderDto);
        return response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.Unauthorized && response.StatusCode != HttpStatusCode.Conflict;
    }
    
    public async Task<bool> UpdateProductionOrderAsync(int id, UpdateProductionOrder updateProductionOrderDto)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return false;
        }
        var response = await httpClient.PostAsJsonAsync($"/api/productionOrder/update/{id}", updateProductionOrderDto);
        return response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.Unauthorized && response.StatusCode != HttpStatusCode.Conflict;
    }
}