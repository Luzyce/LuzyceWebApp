using System.Net.Http.Json;
using Luzyce.Core.Models.Order;

namespace LuzyceWebApp.Services;

public class OrderService
{
    private readonly HttpClient _httpClient;

    public OrderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GetOrdersResponseDto> GetOrdersAsync(int pageNumber)
    {
        var response = await _httpClient.PostAsJsonAsync($"/api/order/{pageNumber}", new { });
        return await response.Content.ReadFromJsonAsync<GetOrdersResponseDto>() ?? new GetOrdersResponseDto();
    }
}
