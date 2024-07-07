using System.Net.Http.Json;
using Luzyce.Core.Models.ProductionOrder;

namespace LuzyceWebApp.Services;

public class ProdPrioritiesService(HttpClient httpClient, TokenValidationService tokenValidationService)
{
    public async Task<GetOrdersPositionsResponse?> GetOrdersPositions()
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return null;
        }
        return await httpClient.GetFromJsonAsync<GetOrdersPositionsResponse>($"/api/productionOrder/positions");
    }
}