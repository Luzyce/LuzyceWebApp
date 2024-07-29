using System.Net.Http.Json;
using Luzyce.Core.Models.ProductionPlan;

namespace LuzyceWebApp.Services;

public class ProductionPlanService(HttpClient httpClient, TokenValidationService tokenValidationService)
{
    public async Task<GetProductionPlans?> GetProductionPlansAsync(GetMonthProductionPlanRequest getMonthProductionPlanRequest)
    {
        if (!await tokenValidationService.IsTokenValid())
        {
            return null;
        }   

        var response = await httpClient.PostAsJsonAsync("/api/productionPlan", getMonthProductionPlanRequest);
        
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<GetProductionPlans>();
        }

        return null;
    }
}