﻿using System.Net;
using System.Net.Http.Json;
using Luzyce.Core.Models.ProductionOrder;
using Luzyce.Core.Models.ProductionPriority;

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
    
    public async Task<bool> SaveProductionPriority(CreateProductionPriorityRequest request)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return false;
        }
        var response = await httpClient.PostAsJsonAsync("/api/productionPriority/save", request);
        return response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.Unauthorized && response.StatusCode != HttpStatusCode.Conflict;
    }
}