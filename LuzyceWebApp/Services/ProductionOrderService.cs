using System.Net.Http.Json;
using Luzyce.Core.Models.ProductionOrder;
using Luzyce.Core.Models.User;

namespace LuzyceWebApp.Services;

public class ProductionOrderService(HttpClient httpClient, TokenValidationService tokenValidationService)
{
    public async Task<GetProductionOrdersResponse?> GetProductionOrdersResponse()
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return null;
        }
        return await httpClient.GetFromJsonAsync<GetProductionOrdersResponse>($"/api/productionOrder");
    }
}