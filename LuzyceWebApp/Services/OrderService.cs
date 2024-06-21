using System.Net.Http.Json;
using Luzyce.Core.Models.Order;
using System.Threading;

namespace LuzyceWebApp.Services;

public class OrderService
{
    private readonly HttpClient _httpClient;

    public OrderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GetOrdersResponseDto> GetOrdersAsync(int pageNumber, string customerName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync($"/api/order/{pageNumber}", new { CustomerName = customerName }, cancellationToken);
        return await response.Content.ReadFromJsonAsync<GetOrdersResponseDto>(cancellationToken) ?? new GetOrdersResponseDto();
    }
}
